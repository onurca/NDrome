using System;
using System.Collections.Generic;
using Ndrome.Common;
using Ndrome.Model.Business;
using Ndrome.Service.Interfaces;

namespace Ndrome.Service.Services
{
    public class ContentDetailService : Service, IContentDetailService
    {
        readonly IRepository<Content> _contentRepository;
        readonly IRepository<ContentDetail> _contentDetailRepository;

        public ContentDetailService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            _contentRepository = UnitOfWork.GetRepository<Content>();
            _contentDetailRepository = UnitOfWork.GetRepository<ContentDetail>();
        }

        public ContentDetail Create(ContentDetail contentDetail)
        {
            if (contentDetail.ContentID == Guid.Empty)
            {
                var content = _contentRepository.Create(new Content());
                contentDetail.ContentID = content.ID;
            }

            contentDetail = _contentDetailRepository.Create(contentDetail);

            UnitOfWork.SaveChanges();

            return contentDetail;
        }

        public void Delete(Guid id)
        {
            _contentDetailRepository.Delete(x => x.ID == id);
        }

        public ContentDetail Get(Guid id)
        {
            return _contentDetailRepository.Get(x => x.ID == id);
        }

        public IEnumerable<ContentDetail> GetAll(Guid contentId)
        {
            return _contentDetailRepository.GetAll(x => x.ContentID == contentId);
        }
    }
}
