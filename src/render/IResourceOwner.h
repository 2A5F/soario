#pragma once
#include "IResource.h"
#include "../utils/IObject.h"

namespace ccc {
    class IResource;

    class IResourceOwner : public IObject {
        size_t m_resource_owner_id;

    public:
        explicit IResourceOwner();

        size_t resource_owner_id() const {
            return m_resource_owner_id;
        }

        bool is_same_owner(const size_t id) const {
            return m_resource_owner_id == id;
        }

        bool is_same_owner(const IResourceOwner &owner) const {
            return m_resource_owner_id == owner.m_resource_owner_id;
        }

        void assert_ownership(const size_t id) const {
            if (!is_same_owner(id))
                throw std::exception("Does not have ownership of this object");
        }

        void assert_ownership(const IResource &resource) const;

        void assert_ownership(const IResource *resource) const {
            assert_ownership(*resource);
        }
    };
} // ccc
