#pragma once
#include <concepts>
#include <exception>

#include "../utils/Object.h"

namespace ccc
{
    class IResource : public virtual Object
    {
        size_t m_resource_owner_id;

    public:
        explicit IResource() = delete;

        explicit IResource(const size_t resource_owner_id): m_resource_owner_id(resource_owner_id)
        {
        }

        size_t resource_owner_id() const
        {
            return m_resource_owner_id;
        }
    };

    class ResourceOwner final : public virtual Object
    {
        size_t m_resource_owner_id;

    public:
        explicit ResourceOwner();

        size_t resource_owner_id() const
        {
            return m_resource_owner_id;
        }

        bool is_same_owner(const size_t id) const
        {
            return m_resource_owner_id == id;
        }

        bool is_same_owner(const ResourceOwner& owner) const
        {
            return m_resource_owner_id == owner.m_resource_owner_id;
        }

        void assert_ownership(const size_t id) const
        {
            if (!is_same_owner(id))
                throw std::exception("Does not have ownership of this object");
        }

        template <class T> requires std::derived_from<T, IResource>
        void assert_ownership(const T& resource) const
        {
            assert_ownership(resource.resource_owner_id());
        }

        template <class T> requires std::derived_from<T, IResource>
        void assert_ownership(const T* resource) const
        {
            assert_ownership(*resource);
        }
    };
} // ccc
