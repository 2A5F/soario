SamplerState _sampler_point_clamp : register(s0, space0);
SamplerState _sampler_point_wrap : register(s1, space0);
SamplerState _sampler_point_mirror : register(s2, space0);

SamplerState _sampler_liner_clamp : register(s3, space0);
SamplerState _sampler_liner_wrap : register(s4, space0);
SamplerState _sampler_liner_mirror : register(s5, space0);

struct BindLessEntry_t {
    uint index;
};

ConstantBuffer<BindLessEntry_t> _bindless_entry : register(b0, space0);

template<typename T>
T LoadRoot() {
    return ResourceDescriptorHeap[_bindless_entry.index];
}

template<typename T>
struct ResRef {
    uint index;

    T get() {
        return ResourceDescriptorHeap[index];
    }
};

struct SamplerRef {
    uint index;

    SamplerState get() {
        return SamplerDescriptorHeap[index];
    }
};
