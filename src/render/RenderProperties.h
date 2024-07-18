#pragma once

#include "parallel_hashmap/phmap.h"
#include "../pch.h"
#include "../utils/Object.h"

namespace ccc {
    class RenderProperties final : public virtual Object {
        phmap::flat_hash_map<int32_t, int32_t> m_int_properties{};
        phmap::flat_hash_map<int32_t, float4> m_vector_properties{};
        phmap::flat_hash_map<int32_t, float4x4> m_matrix_properties{};

        // todo resources

    public:
        void set_int(const int32_t id, const int v) {
            m_int_properties[id] = v;
        }

        void set_vector(const int32_t id, const float4 &v) {
            m_vector_properties[id] = v;
        }

        void set_matrix(const int32_t id, const float4x4 &v) {
            m_matrix_properties[id] = v;
        }
    };
} // ccc
