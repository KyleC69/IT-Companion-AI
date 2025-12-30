using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkKnowledgeBase.Llm;

namespace Tests
{
    [TestClass]
    public sealed class OnnxClientsTests
    {
        [TestMethod]
        public void TrimCompletion_RemovesPromptPrefix()
        {

        }

        [TestMethod]
        public void TrimCompletion_ReturnsTrimmedContentWhenPromptDoesNotMatch()
        {
            
       
        }

        [TestMethod]
        public void ArgMax_ReturnsIndexOfLargestValue()
        {
            var logits = new float[] { -2f, 0.5f, 0.3f, 0.5f, 1.5f };
            var index = OnnxLLMClient.ArgMax(new ArraySegment<float>(logits));

            Assert.AreEqual(4, index);
        }

        [TestMethod]
        public void ParseLayerIndex_ExtractsNumberOrReturnsZero()
        {
            var parsed = OnnxLLMClient.ParseLayerIndex("past_key_values.7.key");
            Assert.AreEqual(7, parsed);

            var fallback = OnnxLLMClient.ParseLayerIndex("past_key_values.layerX.key");
            Assert.AreEqual(0, fallback);
        }

        [TestMethod]
        public void BuildInferenceInputs_WithMasks_BuildsExpectedCoreTensors()
        {
            var tokens = new List<int> { 10, 11, 12 };
            var context = CreateContext(requiresPositionIds: true, requiresAttentionMask: true);

            var inputs = BuildInputs(tokens, context);

            try
            {
                Assert.AreEqual(3, inputs.Count);

                var ids = inputs.Single(i => i.Name == "input_ids").AsTensor<long>().ToArray();
                CollectionAssert.AreEqual(new long[] { 10, 11, 12 }, ids);

                var positions = inputs.Single(i => i.Name == "position_ids").AsTensor<long>().ToArray();
                CollectionAssert.AreEqual(new long[] { 0, 1, 2 }, positions);

                var mask = inputs.Single(i => i.Name == "attention_mask").AsTensor<long>().ToArray();
                CollectionAssert.AreEqual(new long[] { 1, 1, 1 }, mask);
            }
            finally
            {
                DisposeInputs(inputs);
            }
        }

        [TestMethod]
        public void BuildInferenceInputs_WhenKvCachesRequired_AddsPastKeyValues()
        {
            var tokens = new List<int> { 3, 4 };
            var context = CreateContext(
                requiresPastKeyValues: true,
                numPastLayers: 2,
                kvShape: new[] { 1, 2, 0, 4 });

            var inputs = BuildInputs(tokens, context);

            try
            {
                Assert.AreEqual(5, inputs.Count);

                var ids = inputs.Single(i => i.Name == "input_ids").AsTensor<long>().ToArray();
                CollectionAssert.AreEqual(new long[] { 4 }, ids);

                Assert.IsTrue(inputs.Any(i => i.Name == "past_key_values.0.key"));
                Assert.IsTrue(inputs.Any(i => i.Name == "past_key_values.0.value"));
                Assert.IsTrue(inputs.Any(i => i.Name == "past_key_values.1.key"));
                Assert.IsTrue(inputs.Any(i => i.Name == "past_key_values.1.value"));

                foreach (var kvInput in inputs.Where(i => i.Name.StartsWith("past_key_values.", StringComparison.Ordinal)))
                {
                    Assert.AreEqual(0, kvInput.AsTensor<float>().Length);
                }
            }
            finally
            {
                DisposeInputs(inputs);
            }
        }

        [TestMethod]
        public void BuildInferenceInputs_WhenTokensMissing_ThrowsArgumentException()
        {
            var context = CreateContext();
            AssertArgumentException(() => BuildInputs(Array.Empty<int>(), context));
        }

        [TestMethod]
        public void BuildInferenceInputs_WithKvCacheAndAttentionMask_UsesFullContextMask()
        {
            var tokens = new List<int> { 5, 6, 7 };
            var context = CreateContext(
                requiresPastKeyValues: true,
                requiresAttentionMask: true,
                numPastLayers: 1,
                kvShape: new[] { 1, 2, 0, 4 });

            var inputs = BuildInputs(tokens, context);

            try
            {
                var ids = inputs.Single(i => i.Name == "input_ids").AsTensor<long>().ToArray();
                CollectionAssert.AreEqual(new long[] { 7 }, ids, "Incremental mode should feed only the latest token.");

                var mask = inputs.Single(i => i.Name == "attention_mask").AsTensor<long>().ToArray();
                CollectionAssert.AreEqual(new long[] { 1, 1, 1 }, mask, "Attention mask must span the full context.");

                Assert.IsTrue(inputs.Any(i => i.Name == "past_key_values.0.key"));
                Assert.IsTrue(inputs.Any(i => i.Name == "past_key_values.0.value"));

                foreach (var kvInput in inputs.Where(i => i.Name.StartsWith("past_key_values.", StringComparison.Ordinal)))
                {
                    Assert.AreEqual(0, kvInput.AsTensor<float>().Length, "Initial KV cache tensors should be empty.");
                }
            }
            finally
            {
                DisposeInputs(inputs);
            }
        }

        private static OnnxLLMClientInputContext CreateContext(
            bool requiresPastKeyValues = false,
            bool requiresPositionIds = false,
            bool requiresAttentionMask = false,
            int numPastLayers = 0,
            int[]? kvShape = null,
            TensorElementType elementType = TensorElementType.Float)
        {
            return new OnnxLLMClientInputContext(
                requiresPastKeyValues,
                requiresPositionIds,
                requiresAttentionMask,
                "input_ids",
                "position_ids",
                "attention_mask",
                numPastLayers,
                kvShape,
                elementType);
        }

        private static List<NamedOnnxValue> BuildInputs(
            IList<int> tokens,
            OnnxLLMClientInputContext context)
        {
            return OnnxLLMClient.BuildInferenceInputsCore(
                tokens,
                0,
                context,
                pastKeysFloat: null,
                pastValuesFloat: null,
                pastKeysHalf: null,
                pastValuesHalf: null);
        }

        private static void DisposeInputs(IEnumerable<NamedOnnxValue> inputs)
        {
            foreach (var input in inputs)
            {
                if (input is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        private static void AssertArgumentException(Action action)
        {
            try
            {
                action();
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }
    }
}