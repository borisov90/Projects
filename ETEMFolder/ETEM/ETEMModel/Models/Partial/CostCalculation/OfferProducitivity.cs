using ETEMModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models
{
    public partial class OfferProducitivity : Identifiable
    {
        #region Identifiable Members

        public int EntityID
        {
            get { return this.idOfferProducitivity; }
        }

        public string ValidationErrorsAsString { get; set; }

        public List<string> ValidateEntity(CallContext outputContext)
        {
            List<string> result = new List<string>();

            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

            ValidationErrorsAsString = string.Empty;

            return result;
        }




        #endregion


        
        /// <summary>
        /// Decimal, 3 digits =Productivity (kg/MH)/1000
        /// </summary>
        public decimal PackagingProducitivity_TON_MH_Computable
        {

            get
            {   
                try
                {
                    return PackagingProducitivity_KG_MH / 1000;
                }
                catch
                {
                    return Decimal.Zero;
                }
            }
        }

        /// <summary>
        /// Decimal, 3 digits =Productivity (kg/MH)/1000
        /// </summary>
        public decimal PressProducitivity_TON_MH_Computable
        {

            get
            {
                try
                {
                    return PressProducitivity_KG_MH / 1000;
                }
                catch
                {
                    return Decimal.Zero;
                }
            }
        }

        
        /// <summary>
        /// Decimal, 3 digits =Productivity (kg/MH)/1000
        /// </summary>
        public decimal QCProducitivity_TON_MH_Computable
        {

            get
            {
                try
                {
                    return QCProducitivity_KG_MH / 1000;
                }
                catch
                {
                    return Decimal.Zero;
                }
            }
        }

        /// <summary>
        /// Decimal, 3 digits =Productivity (kg/MH)/1000
        /// </summary>
        public decimal COMetalProducitivity_TON_MH_Computable
        {

            get
            {
                try
                {
                    return COMetalProducitivity_KG_MH / 1000;
                }
                catch
                {
                    return Decimal.Zero;
                }
            }
        }
        
        


    }
}