using ETEMModel.Helpers.Admin;
using ETEMModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ETEMModel.Helpers.CostCalculation
{
    public class ExpenseCalculationBL
    {
        private ETEMDataModelEntities dbContext;
       

        public ExpenseCalculationBL()
        {
            dbContext = new ETEMDataModelEntities();
        }
        public List<Expense> LoadExpenseGroupByIdSAPData(int idSAPData)
        {
            List<Expense> listGroupExpense = new List<Expense>();

            SAPData sapData = new SAPDataBL().GetEntityById(idSAPData);

            List<SAPDataExpense>    listSAPDataExpenses = sapData.SAPDataExpenses.ToList();
            List<SAPDataQuantity>   listSAPDataQuantity = sapData.SAPDataQuantities.ToList();

            List<KeyValue> listKeyValueExpense = new KeyValueBL().GetAllKeyValueByKeyTypeIntCode(ETEMEnums.KeyTypeEnum.ExpensesType.ToString());
            List<KeyValue> listKeyValueQuantity = new KeyValueBL().GetAllKeyValueByKeyTypeIntCode(ETEMEnums.KeyTypeEnum.QuantityType.ToString());

            KeyValue kvMachineHours = listKeyValueQuantity.Where(w => w.KeyValueIntCode == ETEMEnums.QuantityTypeEnum.MachineHours.ToString()).FirstOrDefault();
            KeyValue kvProductionQuantity = listKeyValueQuantity.Where(w => w.KeyValueIntCode == ETEMEnums.QuantityTypeEnum.ProductionQuantity.ToString()).FirstOrDefault();

            List<KeyValue> listKeyValueCostCenter = new KeyValueBL().GetAllKeyValueByKeyTypeIntCode(ETEMEnums.KeyTypeEnum.CostCenter.ToString());

            List<string> listIntCodesCostCenterForProductionQuantity = new List<string>()
            {
                ETEMEnums.CostCenterEnum.DIESDepartment.ToString()
            };            
                        
            Dictionary<int, int> dictKvCostCenterIdAndQuantityTypeId = new Dictionary<int, int>();

            foreach (KeyValue kvCostCenter in listKeyValueCostCenter)
            {
                if (listIntCodesCostCenterForProductionQuantity.Contains(kvCostCenter.KeyValueIntCode))
                {
                    if (!dictKvCostCenterIdAndQuantityTypeId.ContainsKey(kvCostCenter.idKeyValue))
                    {
                        dictKvCostCenterIdAndQuantityTypeId.Add(kvCostCenter.idKeyValue, kvProductionQuantity.idKeyValue);
                    }
                }
                else
                {
                    if (!dictKvCostCenterIdAndQuantityTypeId.ContainsKey(kvCostCenter.idKeyValue))
                    {
                        dictKvCostCenterIdAndQuantityTypeId.Add(kvCostCenter.idKeyValue, kvMachineHours.idKeyValue);
                    }
                }
            }

            List<KeyValue> listKeyValueExpensesTypeGroup = new KeyValueBL().GetAllKeyValueByKeyTypeIntCode(ETEMEnums.KeyTypeEnum.ExpensesTypeGroup.ToString());

            //this.expenseGroup = new ExpenseGroup();

            int quantityTypeID = Constants.INVALID_ID;
            foreach (KeyValue costCenter in listKeyValueCostCenter)
            {
                foreach (KeyValue expensesTypeGroup in listKeyValueExpensesTypeGroup)
                {
                    Expense expense = new Expense();
                    expense.IdSAPData = sapData.idSAPData;

                    expense.ExpenseGroup = expensesTypeGroup;
                    expense.CostCenter = costCenter;
                    expense.ExpenseValue = listSAPDataExpenses
                                                .Where(k =>
                                                       k.idCostCenter == costCenter.idKeyValue &&
                                                       listKeyValueExpense
                                                       .Where(f => f.DefaultValue2 == expensesTypeGroup.KeyValueIntCode)
                                                       .Select(f => f.idKeyValue).Contains(k.idExpensesType))
                                                .Sum(k => k.ValueData);

                    if (dictKvCostCenterIdAndQuantityTypeId.ContainsKey(costCenter.idKeyValue))
                    {
                        quantityTypeID = dictKvCostCenterIdAndQuantityTypeId[costCenter.idKeyValue];
                    }

                    SAPDataQuantity quantity = listSAPDataQuantity.FirstOrDefault(q => q.idCostCenter == costCenter.idKeyValue && q.idQuantityType == quantityTypeID);
                    
                    if (quantity != null && quantity.ValueData != Decimal.Zero) 
                    {
                        expense.ExpenseValue_MH = expense.ExpenseValue / quantity.ValueData;
                    }
                    
                    listGroupExpense.Add(expense);
                }
            }

            return listGroupExpense;
        }

        internal CallContext CalculateCostCentersTotal(int idSAPData, CallContext callContext)
        {
            SAPDataCostCenterTotalBL sapDataCostCenterTotalBL = new SAPDataCostCenterTotalBL();
            List<Expense> listGroupExpense = LoadExpenseGroupByIdSAPData(idSAPData);

            List<KeyValue> listKeyValueCostCenter = new KeyValueBL().GetAllKeyValueByKeyTypeIntCode("CostCenter");

            List<SAPDataCostCenterTotal> listTotal = new List<SAPDataCostCenterTotal>();

            List<SAPDataCostCenterTotal> listSAPDataCostCenterTotal_IDS = this.dbContext.SAPDataCostCenterTotals.Where(s => s.idSAPData == idSAPData).Select(s=>s).ToList();

            sapDataCostCenterTotalBL.EntityDelete<SAPDataCostCenterTotalBL>(listSAPDataCostCenterTotal_IDS, callContext);

            foreach (KeyValue costCenter in listKeyValueCostCenter)
            {
                SAPDataCostCenterTotal total = new SAPDataCostCenterTotal();

                total.idCostCenter = costCenter.idKeyValue;
                total.idSAPData = idSAPData;
                total.Total_MH = listGroupExpense.Where(s => s.CostCenter.idKeyValue == costCenter.idKeyValue).Sum(s => s.ExpenseValue_MH);
                listTotal.Add(total);
            }

            callContext = sapDataCostCenterTotalBL.EntitySave<SAPDataCostCenterTotal>(listTotal, callContext);

            return callContext;
        }

        internal void CreateExpenseGroupForOffer(int idOffer)
        {
            Offer offer = new OfferBL().GetEntityById(idOffer);

            SAPData sapData = new SAPDataBL().GetSAPDataByDateActiveTo(offer.OfferDate);
            List<Expense> listExpenseToBeSave = new List<Expense>();



            if (sapData != null)
            {
                List<Expense> list = new ExpenseCalculationBL().LoadExpenseGroupByIdSAPData(sapData.idSAPData);

                

                

                
            }
            else
            {
                throw new ETEMModelException("Not found expenses. ");
            }

        }

        internal List<string> GetPressIntCodeListByDimension(Offer offer)
        {
             DiePriceListDetail diePriceListDetail = new DiePriceListDetailBL().GetEntityById(offer.idDiePriceListDetail.Value);
            string dimensions = diePriceListDetail.DimensionA_String.Trim() + "x" + diePriceListDetail.DimensionB_String.Trim();

            List<KeyValue> listDieDimensions = new KeyValueBL().GetAllKeyValueByKeyTypeIntCode("DieDimensions")
                                                               .Where(k=>k.KeyValueIntCode == dimensions).ToList();

            List<string> keyValueIntCodeCostCenter = new List<string>();

            foreach (KeyValue keyvalue in listDieDimensions)
            {
                keyValueIntCodeCostCenter.AddRange(keyvalue.DefaultValue1.Split(','));//RETURN KEY VALUES OF PROPER PRESS
            }

            keyValueIntCodeCostCenter = keyValueIntCodeCostCenter.Distinct().ToList();//List of press

            return keyValueIntCodeCostCenter;
        }

        internal List<Expense> GetListGroupExpense(Offer offer, CallContext callContext)
        {
            List<Expense> result = new List<Expense>();
            List<string> keyValueIntCodeCostCenter = new List<string>();

            SAPData sapData = new SAPDataBL().GetSAPDataByDateActiveTo(offer.OfferDate);
            if (sapData == null)
            {
                return result;
            }


            keyValueIntCodeCostCenter = GetPressIntCodeListByDimension(offer);
            List<KeyValue> listKeyValueCostCenter = new KeyValueBL().GetAllKeyValueByKeyTypeIntCode("CostCenter");
           

            result = LoadExpenseGroupByIdSAPData(sapData.idSAPData).Where(c=>keyValueIntCodeCostCenter.Contains( c.CostCenter.KeyValueIntCode)).ToList();////PRESS



            Decimal totalExpence = Decimal.Zero;
            string pressNameKeyValue = string.Empty;



            if (offer.idPress.HasValue)
            {
                //ТОВА Е СЛУЧАЙ, пресата на базата на тежестта на Weight per meter (gr/m)
                
                KeyValue kvSelectedPress = listKeyValueCostCenter.Where(k => k.idKeyValue == offer.idPress).FirstOrDefault();

                if (kvSelectedPress == null || !keyValueIntCodeCostCenter.Contains(kvSelectedPress.KeyValueIntCode))
                {
                    return null;
                }

                pressNameKeyValue = listKeyValueCostCenter.Where(k => k.idKeyValue == offer.idPress).FirstOrDefault().KeyValueIntCode;

                
            }
            else
            {
                return null;
                ////ТОВА Е СЛУЧАЙ, пресата се избира на базата на раходите 
                //foreach (string pressName in keyValueIntCodeCostCenter)
                //{
                //    Decimal tmpExpence = result.Where(s => s.CostCenter.KeyValueIntCode == pressName).Sum(s => s.ExpenseValue_MH);
                //    if (totalExpence < tmpExpence)
                //    {
                //        totalExpence = tmpExpence;
                //        pressNameKeyValue = pressName;
                //    }
                //}
            }


            bool includeAging = offer.idAging == new KeyValueBL().GetKeyValueIdByIntCode("YES_NO", "Yes");



            keyValueIntCodeCostCenter.Clear();
            
            listKeyValueCostCenter = listKeyValueCostCenter.Where(k => 
                                    (includeAging? k.DefaultValue2 == pressNameKeyValue : k.KeyValueIntCode == pressNameKeyValue) || 
                                    k.DefaultValue1 == "QualityControl" ||
                                    k.DefaultValue1 == "DIES" ||
                                    k.DefaultValue1 == "Packaging" 
                                    ).ToList();


            foreach (var cc in listKeyValueCostCenter)
            {
                keyValueIntCodeCostCenter.Add(cc.KeyValueIntCode); 
            }
            


            result = LoadExpenseGroupByIdSAPData(sapData.idSAPData).Where(c=>keyValueIntCodeCostCenter.Contains( c.CostCenter.KeyValueIntCode)).ToList();

            return result;


        }
    }

   

    public class Expense
    {
        public int IdSAPData { get; set; }
        public KeyValue CostCenter { get; set; }        
        public KeyValue ExpenseGroup {get;set;}
        public Decimal  ExpenseValue { get; set; }
        public Decimal  ExpenseValueRound
        {
            get
            {
                return Math.Round(this.ExpenseValue, 2, MidpointRounding.AwayFromZero);
            }
        }
        public Decimal ExpenseValueRoundFour
        {
            get
            {
                return Math.Round(this.ExpenseValue, 4, MidpointRounding.AwayFromZero);
            }
        }
        public Decimal  ExpenseValue_MH { get; set; }
        public Decimal ExpenseValue_MH_Round
        {
            get
            {
                return Math.Round(this.ExpenseValue_MH, 2, MidpointRounding.AwayFromZero);
            }
        }
        public Decimal ExpenseValue_MH_RoundFour
        {
            get
            {
                return Math.Round(this.ExpenseValue_MH, 4, MidpointRounding.AwayFromZero);
            }
        }
        public Decimal  ExpenseValue_PER_TON { get; set; }
        public Decimal ExpenseValue_PER_TON_Round
        {
            get
            {
                return Math.Round(this.ExpenseValue_PER_TON, 2, MidpointRounding.AwayFromZero);
            }
        }
        public Decimal ExpenseValue_PER_TON_RoundFour
        {
            get
            {
                return Math.Round(this.ExpenseValue_PER_TON, 4, MidpointRounding.AwayFromZero);
            }
        }
    }   
}