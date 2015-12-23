using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;

namespace ETEMModel.Helpers.Admin
{
    public class KeyValueBL : BaseClassBL<KeyValue>
    {
        public KeyValueBL()
        {
            this.EntitySetName = "KeyValues";
        }

        internal override KeyValue GetEntityById(int idEntity)
        {
            return this.dbContext.KeyValues.Where(e => e.idKeyValue == idEntity).FirstOrDefault();
        }



        internal override void EntityToEntity(KeyValue sourceEntity, KeyValue targetEntity)
        {

            targetEntity.Name = sourceEntity.Name;
            targetEntity.NameEN = sourceEntity.NameEN;
            targetEntity.KeyValueIntCode = sourceEntity.KeyValueIntCode;
            targetEntity.Description = sourceEntity.Description;
            targetEntity.V_Order = sourceEntity.V_Order;

            targetEntity.DefaultValue1 = sourceEntity.DefaultValue1;
            targetEntity.DefaultValue2 = sourceEntity.DefaultValue2;
            targetEntity.DefaultValue3 = sourceEntity.DefaultValue3;
            targetEntity.DefaultValue4 = sourceEntity.DefaultValue4;
            targetEntity.DefaultValue5 = sourceEntity.DefaultValue5;
            targetEntity.DefaultValue6 = sourceEntity.DefaultValue6;

            targetEntity.CodeAdminUNI = sourceEntity.CodeAdminUNI;


        }

        internal int GetKeyValueIdByIntCode(string _IntCodeType, string _IntCodeValue)
        {
            return GetKeyValueByIntCode(_IntCodeType, _IntCodeValue).idKeyValue;
        }




        internal KeyValue GetKeyValueByIntCode(string _IntCodeType, string _IntCodeValue)
        {
            var nKeyValue = from kv in this.dbContext.KeyValues
                            join kt in this.dbContext.KeyTypes on kv.idKeyType equals kt.idKeyType
                            where kt.KeyTypeIntCode.Trim() == _IntCodeType && kv.KeyValueIntCode.Trim() == _IntCodeValue
                            select kv;

            if (nKeyValue.Count() == 1)
            {
                return nKeyValue.First();
            }
            else
            {
                return new KeyValue();
            }
        }

        internal List<KeyValue> GetAllKeyValueByKeyTypeID(string _entityID, string sortExpression, string sortDirection)
        {
            int entityID;
            if (Int32.TryParse(_entityID, out entityID))
            {
                List<KeyValue> listKeyValues = dbContext.KeyValues.Where(e => e.idKeyType == entityID).ToList();
                listKeyValues = BaseClassBL<KeyValue>.Sort(listKeyValues, sortExpression, sortDirection).ToList();
                return listKeyValues;
            }
            else
            {
                return null;
            }
        }

        internal KeyValue GetKeyValueByKeyValueID(string _entityID)
        {
            int entityID;
            if (Int32.TryParse(_entityID, out entityID))
            {
                return dbContext.KeyValues.Where(e => e.idKeyValue == entityID).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        internal List<KeyValue> GetAllKeyValueByKeyTypeIntCode(string keyTypeIntCode)
        {
            return dbContext.KeyValues.Where(e => e.KeyType.KeyTypeIntCode == keyTypeIntCode).ToList();
        }



        internal List<KeyValue> GetKeyValuesByKeyTypeId(int idKeyType)
        {
            List<KeyValue> keyValues = dbContext.KeyValues.Where(k => k.idKeyType == idKeyType).ToList();
            return keyValues;
        }
    }
}