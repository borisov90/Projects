using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;

namespace ETEMModel.Helpers.Admin
{
    public class LocationBL : BaseClassBL<Location>
    {
        public LocationBL()
        {
            this.EntitySetName = "Locations";
        }

        internal override Location GetEntityById(int idEntity)
        {
            return this.dbContext.Locations.Where(l => l.idLocation == idEntity).FirstOrDefault();
        }

        internal override void EntityToEntity(Location sourceEntity, Location targetEntity)
        {
            targetEntity.Name = sourceEntity.Name;
            targetEntity.LocationName = sourceEntity.LocationName;
            targetEntity.LocationCode = sourceEntity.LocationCode;
            targetEntity.idVillageType = sourceEntity.idVillageType;
            targetEntity.idMunicipality = sourceEntity.idMunicipality;
            targetEntity.OrderValue = sourceEntity.OrderValue;
        }

        public Location GetLocationById(int idEntity)
        {
            return GetEntityById(idEntity);
        }

        public LocationView GetLocationViewById(int idEntity)
        {
            return this.dbContext.LocationViews.Where(l => l.idLocation == idEntity).FirstOrDefault();
        }

        public List<Location> GetAllLocations()
        {
            List<Location> list = GetAllEntities<Location>();
            list = BaseClassBL<Location>.Sort(list, "Name", Constants.SORTING_ASC).ToList();
            return list;
        }

        public List<LocationView> GetAllLocationViews()
        {
            List<LocationView> list = (from lv in this.dbContext.LocationViews
                                       orderby lv.Name ascending
                                       select lv).ToList<LocationView>();

            return list;
        }

        public List<Location> GetLocationsByMunicipalityId(int municipalityID)
        {
            List<Location> list = (from l in this.dbContext.Locations
                                   where l.idMunicipality == municipalityID
                                   orderby l.Name ascending
                                   select l).ToList<Location>();

            return list;
        }

        public List<LocationView> GetLocationViewsByMunicipalityId(int municipalityID)
        {
            List<LocationView> list = (from lv in this.dbContext.LocationViews
                                       where lv.idMunicipality == municipalityID
                                       orderby lv.Name ascending
                                       select lv).ToList<LocationView>();

            return list;
        }

        public EKATTE GetEKATTEById(int idEntity)
        {
            return this.dbContext.EKATTEs.Where(e => e.idEKATTE == idEntity).FirstOrDefault();
        }

        public List<EKATTE> GetAllEKATTEs()
        {
            List<EKATTE> list = (from ek in this.dbContext.EKATTEs
                                 orderby ek.Name ascending
                                 select ek).ToList<EKATTE>();

            return list;
        }

        public List<EKATTE> GetEKATTEsByMunicipalityId(int municipalityID)
        {
            List<EKATTE> list = (from ek in this.dbContext.EKATTEs
                                 where ek.idMunicipality == municipalityID
                                 orderby ek.Name ascending
                                 select ek).ToList<EKATTE>();

            return list;
        }

        public Municipality GetMunicipalityById(int idEntity)
        {
            return this.dbContext.Municipalities.Where(m => m.idMunicipality == idEntity).FirstOrDefault();
        }

        public List<Municipality> GetAllMunicipalities()
        {
            List<Municipality> list = (from m in this.dbContext.Municipalities
                                       select m).OrderBy(s => s.OrderValue).ThenBy(s => s.Name).ToList<Municipality>();

            return list;
        }

        public List<Municipality> GetMunicipalitiesByDistrictId(int districtID)
        {
            List<Municipality> list = (from m in this.dbContext.Municipalities
                                       where m.idDistrict == districtID
                                       select m).OrderBy(s => s.OrderValue).ThenBy(s => s.Name).ToList<Municipality>();

            return list;
        }

        public List<Municipality> GetMunicipalitiesByCountryId(int countryID)
        {
            List<Municipality> list = new List<Municipality>();

            var country = GetCountryById(countryID);

            if (country == null)
            {
                return list;
            }

            string codeFRN = ETEMEnums.DistrictCodeEnum.FRN.ToString();
            string codeNAN = ETEMEnums.DistrictCodeEnum.NAN.ToString();
            if (country.CodeISO3166.Trim().ToUpper() == ETEMEnums.CountryCodeEnum.BG.ToString())
            {
                list = (from m in this.dbContext.Municipalities
                        join d in this.dbContext.Districts on m.idDistrict equals d.idDistrict
                        where d.DistrictCode != codeFRN && d.DistrictCode != codeNAN
                        select m).OrderBy(s => s.OrderValue).ThenBy(s => s.Name).ToList<Municipality>();
            }
            else if (country.CodeISO3166.Trim().ToUpper() == ETEMEnums.CountryCodeEnum.NONE.ToString())
            {
                list = (from m in this.dbContext.Municipalities
                        join d in this.dbContext.Districts on m.idDistrict equals d.idDistrict
                        where d.DistrictCode == codeNAN
                        orderby m.Name ascending
                        select m).OrderBy(s => s.OrderValue).ThenBy(s => s.Name).ToList<Municipality>();
            }
            else
            {
                list = (from m in this.dbContext.Municipalities
                        join d in this.dbContext.Districts on m.idDistrict equals d.idDistrict
                        where d.DistrictCode == codeFRN
                        orderby m.Name ascending
                        select m).OrderBy(s => s.OrderValue).ThenBy(s => s.Name).ToList<Municipality>();
            }

            return list;
        }

        public District GetDistrictById(int idEntity)
        {
            return this.dbContext.Districts.Where(d => d.idDistrict == idEntity).FirstOrDefault();
        }

        public District GetDistrictByCode(string districtCode)
        {
            return this.dbContext.Districts.Where(d => d.DistrictCode == districtCode).FirstOrDefault();
        }

        public List<District> GetAllDistricts()
        {
            List<District> list = (from m in this.dbContext.Districts
                                   orderby m.Name ascending
                                   select m).ToList<District>();

            return list;
        }

        public List<District> GetDistrictsByCountryID(string countryID)
        {
            List<District> list = new List<District>();

            var country = GetCountryById(Int32.Parse(countryID));

            if (country == null)
            {
                return list;
            }

            string codeFRN = ETEMEnums.DistrictCodeEnum.FRN.ToString();
            string codeNAN = ETEMEnums.DistrictCodeEnum.NAN.ToString();
            if (country.CodeISO3166.Trim().ToUpper() == ETEMEnums.CountryCodeEnum.BG.ToString())
            {
                list = (from d in this.dbContext.Districts
                        where d.DistrictCode != codeFRN
                        orderby d.Name ascending
                        select d).ToList<District>();
            }
            else if (country.CodeISO3166.Trim().ToUpper() == ETEMEnums.CountryCodeEnum.NONE.ToString())
            {
                list = (from d in this.dbContext.Districts
                        where d.DistrictCode == codeNAN
                        orderby d.Name ascending
                        select d).ToList<District>();
            }
            else
            {
                list = (from d in this.dbContext.Districts
                        where d.DistrictCode == codeFRN
                        orderby d.Name ascending
                        select d).ToList<District>();
            }

            return list;
        }

        public Country GetCountryById(int idEntity)
        {
            return this.dbContext.Countries.Where(c => c.idCountry == idEntity).FirstOrDefault();
        }

        public Country GetCountryByCode(string countryCode)
        {
            return this.dbContext.Countries.Where(c => c.CodeISO3166 == countryCode).FirstOrDefault();
        }

        public List<Country> GetAllCountries()
        {
            List<Country> list = (from c in this.dbContext.Countries
                                  orderby c.Name ascending
                                  select c).ToList<Country>();

            return list;
        }
    }
}