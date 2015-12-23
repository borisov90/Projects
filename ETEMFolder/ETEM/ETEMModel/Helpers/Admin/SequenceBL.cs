using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;

namespace ETEMModel.Helpers.Admin
{
    public class SequenceBL : BaseClassBL<Sequence>
    {
        public SequenceBL()
        {
            this.EntitySetName = "Sequences";
        }

        internal override void EntityToEntity(Sequence sourceEntity, Sequence targetEntity)
        {
            sourceEntity.Resource = targetEntity.Resource;
            sourceEntity.idResource = targetEntity.idResource;
            sourceEntity.Year = targetEntity.Year;
            sourceEntity.nextVal = targetEntity.nextVal;
        }

        internal override Sequence GetEntityById(int idEntity)
        {
            return this.dbContext.Sequences.Where(w => w.idSequence == idEntity).FirstOrDefault();
        }

        public int GetSequenceNextValue(string resource, int? idResource, int? year, CallContext resultContext)
        {
            int result = 0;

            Sequence sequence = new Sequence();

            if (idResource.HasValue && year.HasValue)
            {
                sequence = (from s in this.dbContext.Sequences
                            where s.Resource == resource && s.idResource == idResource && s.Year == year
                            select s).FirstOrDefault();
            }
            else if (idResource.HasValue)
            {
                sequence = (from s in this.dbContext.Sequences
                            where s.Resource == resource && s.idResource == idResource
                            select s).FirstOrDefault();
            }
            else if (year.HasValue)
            {
                sequence = (from s in this.dbContext.Sequences
                            where s.Resource == resource && s.Year == year
                            select s).FirstOrDefault();
            }
            else
            {
                sequence = (from s in this.dbContext.Sequences
                            where s.Resource == resource
                            select s).FirstOrDefault();
            }

            if (sequence != null)
            {
                sequence.nextVal++;
                result = sequence.nextVal;

                base.EntitySave<Sequence>(sequence, resultContext);
            }
            else
            {
                result = 1;

                Sequence sequenceToSave = new Sequence();

                sequenceToSave.Resource = resource;
                sequenceToSave.idResource = idResource;
                sequenceToSave.Year = year;
                sequenceToSave.nextVal = result;

                base.EntitySave<Sequence>(sequenceToSave, resultContext);
            }

            return result;
        }

        public string GetFileVersion(string fileName)
        {
            string result = string.Empty;
            CallContext resultcontext = new CallContext();

            try
            {

                int nextVal = GetSequenceNextValue(fileName,
                                                    null,
                                                    null,
                                                    resultcontext);


                int maska = 10000;
                int revCode = maska + nextVal;

                string[] fileNameExt = fileName.Split('.');
                result = fileNameExt[0] + "_" + "Rev_" + revCode.ToString().Substring(1, 4) + "." + fileNameExt[1];
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка при изграждане на версия за файл - метод 'GetFileVersion', форма 'UploadFile.aspx'!" + ex.ToString());
                BaseHelper.LogToMail("Грешка при изграждане на версия за файл - метод 'GetFileVersion', форма 'UploadFile.aspx'!" + ex.ToString());

            }

            return result;
        }

        public string GetInquiryNumber()
        {
            string result = string.Empty;
            CallContext resultcontext = new CallContext();

            try
            {

                int nextVal = GetSequenceNextValue("InquiryNumber",
                                                    null,
                                                    null,
                                                    resultcontext);


                int maska = 1000000;
                int inquiryNumber = maska + nextVal;

                
                result = inquiryNumber.ToString().Substring(1, 6);
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка при изграждане на версия за файл - метод 'GetFileVersion', форма 'UploadFile.aspx'!" + ex.ToString());
                BaseHelper.LogToMail("Грешка при изграждане на версия за файл - метод 'GetFileVersion', форма 'UploadFile.aspx'!" + ex.ToString());

            }

            return result;
        }
    }
}