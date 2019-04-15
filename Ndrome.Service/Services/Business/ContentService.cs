using System;
using System.Collections.Generic;
using Ndrome.Common;
using Ndrome.Model.Business;
using Ndrome.Service.Interfaces;

namespace Ndrome.Service.Services
{
    public class ContentService : Service, IContentService
    {
        readonly IRepository<Content> _contentRepository;

        public ContentService()
        {
            _contentRepository = UnitOfWork.GetRepository<Content>();
        }

        public Content Create(Content content)
        {
            return _contentRepository.Create(content);
        }

        public void Update(Content content)
        {
            _contentRepository.Update(content);
        }

        public void Delete(Guid id)
        {
            _contentRepository.Delete(x => x.ID == id);
        }

        public Content Get(Guid id)
        {
            return _contentRepository.Get(x => x.ID == id);
        }

        public IEnumerable<Content> GetAll()
        {
            return _contentRepository.GetAll();
        }
    }
}
