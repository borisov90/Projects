using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models.DataView;

namespace ETEMModel.Helpers.Admin
{
    public class KeyTypeBL : BaseClassBL<KeyType>
    {

        public KeyTypeBL()
        {
            this.EntitySetName = "KeyTypes";
        }

        internal override KeyType GetEntityById(int idEntity)
        {
            return this.dbContext.KeyTypes.Where(e => e.idKeyType == idEntity).FirstOrDefault();
        }

        internal KeyType GetKeyTypeByIntCode(string keyTypeIntCode)
        {

            return dbContext.KeyTypes.Where(e => e.KeyTypeIntCode == keyTypeIntCode).FirstOrDefault();
        }

        internal override void EntityToEntity(KeyType entity, KeyType saveEntity)
        {

            saveEntity.Name = entity.Name;
            saveEntity.KeyTypeIntCode = entity.KeyTypeIntCode;
            saveEntity.Description = entity.Description;
            saveEntity.IsSystem = entity.IsSystem;
        }


        public List<KeyType> GetAll(string sortExpression, string sortDirection)
        {
            List<KeyType> list = GetAllEntities<KeyType>();
            list = KeyTypeBL.Sort(list, sortExpression, sortDirection).ToList();

            return list;


        }

        internal List<KeyTypeDataView> GetKeyTypeDataView(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {

            List<KeyTypeDataView> listView = (from kt in dbContext.KeyTypes
                                              join kv in dbContext.KeyValues on kt.idKeyType equals kv.idKeyType into groupKV
                                              from subKeyValue in groupKV.DefaultIfEmpty()

                                              select new KeyTypeDataView
                                              {
                                                  idKeyType = kt.idKeyType,
                                                  Name = kt.Name,
                                                  Description = kt.Description,
                                                  KeyTypeIntCode = kt.KeyTypeIntCode,
                                                  IsSystem = kt.IsSystem,
                                                  LAST_UPD = kt.LAST_UPD,
                                                  KeyValueNameStr = subKeyValue.Name,
                                                  KeyValueIntCodeStr = subKeyValue.KeyValueIntCode,
                                                  KeyValueDescriptionStr = subKeyValue.Description
                                              }
                                            ).Distinct().ApplySearchCriterias(searchCriteria).ToList();

            listView = BaseClassBL<KeyTypeDataView>.Sort(listView, sortExpression, sortDirection).ToList();
            return listView;
        }


    }
}