#include "IResource.h"
namespace ccc {
    namespace {
        std::atomic_size_t s_resource_owner_id_inc;
    }

    ResourceOwner::ResourceOwner(): m_resource_owner_id(s_resource_owner_id_inc++) {
    }

} // ccc
