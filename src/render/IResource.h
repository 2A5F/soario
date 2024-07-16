#pragma once
#include "../utils/IObject.h"

namespace ccc {
    class IResource : public virtual IObject {
        size_t m_resource_owner_id;

    public:
        explicit IResource() = delete;

        explicit IResource(const size_t resource_owner_id): m_resource_owner_id(resource_owner_id) {
        }

        size_t resource_owner_id() const {
            return m_resource_owner_id;
        }
    };
} // ccc
