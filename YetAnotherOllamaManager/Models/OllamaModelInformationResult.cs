namespace YetAnotherOllamaManager.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class ModelInfoDetails
    {
        [JsonPropertyName("parent_model")]
        public string ParentModel { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("family")]
        public string Family { get; set; }

        [JsonPropertyName("families")]
        public List<string> Families { get; set; }

        [JsonPropertyName("parameter_size")]
        public string ParameterSize { get; set; }

        [JsonPropertyName("quantization_level")]
        public string QuantizationLevel { get; set; }
    }

    public class ModelInfo
    {
        [JsonPropertyName("general.architecture")]
        public string GeneralArchitecture { get; set; }

        [JsonPropertyName("general.file_type")]
        public int? GeneralFileType { get; set; }

        [JsonPropertyName("general.parameter_count")]
        public long? GeneralParameterCount { get; set; }

        [JsonPropertyName("general.quantization_version")]
        public int? GeneralQuantizationVersion { get; set; }

        [JsonPropertyName("llama.attention.head_count")]
        public int? LlamaAttentionHeadCount { get; set; }

        [JsonPropertyName("llama.attention.head_count_kv")]
        public int? LlamaAttentionHeadCountKv { get; set; }

        [JsonPropertyName("llama.attention.layer_norm_rms_epsilon")]
        public double? LlamaAttentionLayerNormRmsEpsilon { get; set; }

        [JsonPropertyName("llama.block_count")]
        public int? LlamaBlockCount { get; set; }

        [JsonPropertyName("llama.context_length")]
        public int? LlamaContextLength { get; set; }

        [JsonPropertyName("llama.embedding_length")]
        public int? LlamaEmbeddingLength { get; set; }

        [JsonPropertyName("llama.feed_forward_length")]
        public int? LlamaFeedForwardLength { get; set; }

        [JsonPropertyName("llama.rope.dimension_count")]
        public int? LlamaRopeDimensionCount { get; set; }

        [JsonPropertyName("llama.rope.freq_base")]
        public int? LlamaRopeFreqBase { get; set; }

        [JsonPropertyName("llama.vocab_size")]
        public int? LlamaVocabSize { get; set; }

        [JsonPropertyName("tokenizer.ggml.bos_token_id")]
        public int? TokenizerGgmlBosTokenId { get; set; }

        [JsonPropertyName("tokenizer.ggml.eos_token_id")]
        public int? TokenizerGgmlEosTokenId { get; set; }

        [JsonPropertyName("tokenizer.ggml.merges")]
        public List<object> TokenizerGgmlMerges { get; set; }

        [JsonPropertyName("tokenizer.ggml.model")]
        public string TokenizerGgmlModel { get; set; }

        [JsonPropertyName("tokenizer.ggml.pre")]
        public string TokenizerGgmlPre { get; set; }

        [JsonPropertyName("tokenizer.ggml.token_type")]
        public List<object> TokenizerGgmlTokenType { get; set; }

        [JsonPropertyName("tokenizer.ggml.tokens")]
        public List<object> TokenizerGgmlTokens { get; set; }
    }

    public class OllamaModelInformationResult
    {
        [JsonPropertyName("modelfile")]
        public string Modelfile { get; set; }

        [JsonPropertyName("parameters")]
        public string Parameters { get; set; }

        [JsonPropertyName("template")]
        public string Template { get; set; }

        [JsonPropertyName("details")]
        public ModelInfoDetails Details { get; set; }

        [JsonPropertyName("model_info")]
        public ModelInfo ModelInfo { get; set; }
    }


