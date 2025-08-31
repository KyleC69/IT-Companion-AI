// Project Name: LightweightAI.Core
// File Name: AppLogIngest.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Loaders.Events;



namespace LightweightAI.Core.Loaders;




    /// <summary>
    ///     Ingests structured or semi-structured application logs from 3rd-party sources.
    ///     Supports pluggable parsers and emits standardized telemetry events.
    /// </summary>
    public class AppLogIngest(IAppLogParser parser, string logDirectory, ITelemetryEmitter emitter, IAuditLogger audit)
        : IIntakeDriver
    {
        private readonly IAuditLogger _audit = audit ?? throw new ArgumentNullException(nameof(audit));
        private readonly ITelemetryEmitter _emitter = emitter ?? throw new ArgumentNullException(nameof(emitter));
        private readonly string _logDirectory = logDirectory ?? throw new ArgumentNullException(nameof(logDirectory));
        private readonly IAppLogParser _parser = parser ?? throw new ArgumentNullException(nameof(parser));





        public async Task ExecuteAsync()
        {
            if (!Directory.Exists(this._logDirectory))
            {
                this._audit.Warn($"[AppLogIngest] Directory not found: {this._logDirectory}");
                return;
            }

            var files = Directory.GetFiles(this._logDirectory, "*.log", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
                try
                {
                    var lines = await File.ReadAllLinesAsync(file);
                    foreach (var line in lines)
                    {
                        var evt = this._parser.Parse(line);
                        if (evt != null)
                            this._emitter.Emit(evt);
                        else
                            this._audit.Trace($"[AppLogIngest] Skipped malformed line in {Path.GetFileName(file)}");
                    }

                    this._audit.Info($"[AppLogIngest] Successfully ingested {Path.GetFileName(file)}");
                }
                catch (Exception ex)
                {
                    this._audit.Error($"[AppLogIngest] Failed to ingest {Path.GetFileName(file)}", ex);
                }
        }
    }


