using System;
using System.Collections.Generic;
using Ndrome.Model.Business;

namespace Ndrome.Service.Interfaces
{
    public interface IContentDetailService
    {
        ContentDetail Get(Guid id);
        IEnumerable<ContentDetail> GetAll(Guid contentId);
        ContentDetail Create(ContentDetail content);
        void Delete(Guid id);
    }
}
