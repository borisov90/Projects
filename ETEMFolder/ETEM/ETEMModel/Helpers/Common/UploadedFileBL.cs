using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Helpers.AbstractSearchBLHolder;

namespace ETEMModel.Helpers.Common
{
    public class UploadedFileBL : BaseClassBL<UploadedFile>
    {
        public UploadedFileBL()
        {
            this.EntitySetName = "UploadedFiles";
        }

        internal override void EntityToEntity(UploadedFile entity, UploadedFile saveEntity)
        {
            saveEntity.idResource = entity.idResource;
            saveEntity.ResourceName = entity.ResourceName;
            saveEntity.FilePath = entity.FilePath;
            saveEntity.FileName = entity.FileName;
            saveEntity.Description = entity.Description;
            saveEntity.Extension = entity.Extension;
            saveEntity.DateUpload = entity.DateUpload;
            saveEntity.Size = entity.Size;
            saveEntity.ContentType = entity.ContentType;
        }

        internal override UploadedFile GetEntityById(int idEntity)
        {
            return this.dbContext.UploadedFiles.Where(e => e.idUploadedFile == idEntity).FirstOrDefault();
        }

        public List<UploadedFile> GetUploadedFile(int idPerson)
        {
            List<UploadedFile> fileList = (from uf in dbContext.UploadedFiles
                                           where uf.idResource == idPerson
                                           select uf).ToList();

            return fileList;
        }
    }
}