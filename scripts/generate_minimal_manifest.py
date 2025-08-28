#!/usr/bin/env python3
"""
Minimal File Manifest Generator
================================
Generates a *clone-like* linear manifest of repository files for external AI ingestion.

Outputs:
 1. docs/FILE_MANIFEST.md  (updates auto-generated sections ONLY)
 2. manifest/file-paths.txt   (relative paths, canonical sorted order)
 3. manifest/html-urls.txt    (HTML blob URLs for browsing / fallback)
 4. manifest/raw-urls.txt     (Raw content URLs for direct ingestion)

Design Principles:
 - Deterministic ordering (sorted ASCII) for easy diffing & caching.
 - Zero extra metadata (no hashes / line counts) per user request.
 - Plain regex-based exclusion for simplicity.

Parameters (edit these constants):
 - REPO:        GitHub org/repo slug used to construct URLs
 - BRANCH:      Branch name to anchor blob/raw URLs
 - PATH_PREFIX: Only include files whose path starts with this (default 'src/')
 - EXCLUDE_PATTERNS: List of regex fragments. Any match => file excluded.

Placeholders in docs/FILE_MANIFEST.md are identified by exact blocks:
```
<!-- AUTO-GENERATED: do not edit manually -->
(TREE WILL BE INSERTED HERE BY SCRIPT)
```
(etc. for PATH / HTML / RAW sections)

Usage:
  $ python scripts/generate_minimal_manifest.py

Prereqs:
  - Python 3.9+
  - Git CLI accessible in PATH
  - Executed from any directory (script resolves repo root)

Exit Codes:
  0  success
  1  template missing
  other: git or IO errors

Customization Examples:
  * Include full repo: set PATH_PREFIX = "" and adjust logic accordingly.
  * Add exclusion for coverage data: EXCLUDE_PATTERNS.append(r"/coverage/")
  * Switch to main branch: BRANCH = "main" (and update workflow trigger)

"""
from __future__ import annotations
import subprocess, sys, re, datetime
from pathlib import Path
from typing import List

# ------------------------- Configuration ------------------------- #
REPO = "KyleC69/IT-Companion-AI"    # org/repo slug
BRANCH = "master"                   # Branch used for URLs (auto-detected if "auto")
PATH_PREFIX = "src/"                # Only include paths starting with this prefix (leave empty to include all)
EXCLUDE_PATTERNS = [                # Regex patterns; if *any* matches the full path, it is excluded
    r"/bin/",
    r"/obj/",
    r"/\.git"   # any .git* segment
]
# ---------------------------------------------------------------- #

ROOT = Path(__file__).resolve().parent.parent
DOC = ROOT / "docs" / "FILE_MANIFEST.md"
MANIFEST_DIR = ROOT / "manifest"

SECTION_PATTERNS = {
    "tree":  r"```\n<!-- AUTO-GENERATED: do not edit manually -->\n\(TREE WILL BE INSERTED HERE BY SCRIPT\)\n```",
    "paths": r"```\n<!-- AUTO-GENERATED: do not edit manually -->\n\(PATH LIST WILL BE INSERTED HERE\)\n```",
    "html":  r"```\n<!-- AUTO-GENERATED: do not edit manually -->\n\(HTML URL LIST WILL BE INSERTED HERE\)\n```",
    "raw":   r"```\n<!-- AUTO-GENERATED: do not edit manually -->\n\(RAW URL LIST WILL BE INSERTED HERE\)\n```",
}

# ------------------------- Helpers ------------------------------ #
def run(cmd: List[str]) -> str:
    return subprocess.check_output(cmd, cwd=ROOT, text=True)

def get_branch() -> str:
    """Get the branch name to use for URLs. Auto-detect if BRANCH is 'auto'."""
    if BRANCH != "auto":
        return BRANCH
    
    # Try to get the default branch from git
    try:
        # First try to get the default branch from origin
        result = run(["git", "symbolic-ref", "refs/remotes/origin/HEAD"])
        return result.strip().split("/")[-1]
    except subprocess.CalledProcessError:
        pass
    
    try:
        # Fallback: try to get the current branch
        result = run(["git", "branch", "--show-current"])
        if result.strip():
            return result.strip()
    except subprocess.CalledProcessError:
        pass
    
    # Final fallback
    return "master"

def is_excluded(path: str) -> bool:
    return any(re.search(pat, path) for pat in EXCLUDE_PATTERNS)

def collect_paths() -> List[str]:
    """Return sorted list of tracked file paths honoring prefix & exclusions."""
    all_files = run(["git", "ls-files"]).splitlines()
    selected = []
    for p in all_files:
        if PATH_PREFIX and not p.startswith(PATH_PREFIX):
            continue
        if is_excluded(p):
            continue
        selected.append(p)
    return sorted(selected)

def build_tree(paths: List[str]) -> str:
    tree = {}
    for p in paths:
        parts = p.split("/")
        node = tree
        for seg in parts[:-1]:
            node = node.setdefault(seg, {})
        node.setdefault("__files__", []).append(parts[-1])
    lines = []
    def walk(node, prefix=""):
        dirs = sorted([k for k in node.keys() if k != "__files__"])
        files = sorted(node.get("__files__", []))
        for d in dirs:
            lines.append(f"{prefix}{d}/")
            walk(node[d], prefix + "  ")
        for f in files:
            lines.append(f"{prefix}{f}")
    walk(tree)
    return "\n".join(lines)

def write_plain_lists(paths: List[str]):
    branch = get_branch()
    MANIFEST_DIR.mkdir(parents=True, exist_ok=True)
    (MANIFEST_DIR / "file-paths.txt").write_text("\n".join(paths) + "\n", encoding="utf-8")
    html_urls = [f"https://github.com/{REPO}/blob/{branch}/{p}" for p in paths]
    raw_urls = [f"https://raw.githubusercontent.com/{REPO}/{branch}/{p}" for p in paths]
    (MANIFEST_DIR / "html-urls.txt").write_text("\n".join(html_urls) + "\n", encoding="utf-8")
    (MANIFEST_DIR / "raw-urls.txt").write_text("\n".join(raw_urls) + "\n", encoding="utf-8")
    return html_urls, raw_urls

def replace_section(text: str, kind: str, payload: str) -> str:
    pattern = SECTION_PATTERNS[kind]
    replacement = f"```\n<!-- AUTO-GENERATED: do not edit manually -->\n{payload}\n```"
    new_text, n = re.subn(pattern, replacement, text, count=1, flags=re.MULTILINE)
    if n == 0:
        print(f"[WARN] Placeholder for section '{kind}' not found.")
    return new_text

def stamp_timestamp(text: str) -> str:
    iso = datetime.datetime.now(datetime.timezone.utc).isoformat(timespec="seconds").replace("+00:00", "Z")
    if "Generated:" in text:
        return re.sub(r"Generated:.*$", f"Generated: {iso}", text, flags=re.MULTILINE)
    return text + f"\n\nGenerated: {iso}\n"

# ------------------------- Main Flow ---------------------------- #
def main() -> int:
    if not DOC.exists():
        print(f"[ERROR] Template doc missing: {DOC}")
        return 1
    paths = collect_paths()
    html_urls, raw_urls = write_plain_lists(paths)
    tree = build_tree(paths)

    doc_text = DOC.read_text(encoding="utf-8")
    doc_text = replace_section(doc_text, "tree", tree)
    doc_text = replace_section(doc_text, "paths", "\n".join(paths))
    doc_text = replace_section(doc_text, "html", "\n".join(html_urls))
    doc_text = replace_section(doc_text, "raw", "\n".join(raw_urls))
    doc_text = stamp_timestamp(doc_text)
    DOC.write_text(doc_text, encoding="utf-8")

    print(f"[OK] Updated {DOC}")
    print(f"[OK] {len(paths)} paths written.")
    return 0

if __name__ == "__main__":
    sys.exit(main())