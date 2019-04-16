using Ndrome.Model.Business;
using System;
using System.Collections.Generic;

namespace Ndrome.Service.Interfaces
{
    public interface IContentService
    {
        Content Get(Guid id);
        IEnumerable<Content> GetAll();
        Content Create(Content content);
        void Update(Content content);
        void Delete(Guid id);
    }
}
