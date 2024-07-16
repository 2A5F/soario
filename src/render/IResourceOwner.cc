#include "IResourceOwner.h"

namespace ccc {
    namespace {
        std::atomic_size_t s_resource_owner_id_inc;
    }

    IResourceOwner::IResourceOwner(): m_resource_owner_id(s_resource_owner_id_inc++) {
    }

    void IResourceOwner::assert_ownership(const IResource &resource) const {
        assert_ownership(resource.resource_owner_id());
    }
} // ccc
