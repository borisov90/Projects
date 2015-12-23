using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Models.DataView;
using System.Xml;

namespace ETEMModel.Helpers.Admin
{
    public class MenuNodeBL : BaseClassBL<MenuNode>
    {
        private List<MenuNodeDataView> listChapters;
        private XmlDocument xmlDoc;
        private int depth;
        private string path;

        public MenuNodeBL()
        {
            this.EntitySetName = "MenuNodes";

            listChapters = new List<MenuNodeDataView>();
            this.xmlDoc = new XmlDocument();
        }



        internal override MenuNode GetEntityById(int idEntity)
        {
            return this.dbContext.MenuNodes.Where(e => e.idNode == idEntity).FirstOrDefault();
        }

        internal override void EntityToEntity(MenuNode sourceEntity, MenuNode targetEntity)
        {
            targetEntity.parentNode = sourceEntity.parentNode != null ? sourceEntity.parentNode : targetEntity.parentNode;
            targetEntity.name = sourceEntity.name;
            targetEntity.prevNode = sourceEntity.prevNode != null ? sourceEntity.prevNode : targetEntity.prevNode;
            targetEntity.nextNode = sourceEntity.nextNode != null ? sourceEntity.nextNode : targetEntity.nextNode;
            targetEntity.idNavURL = sourceEntity.idNavURL != null ? sourceEntity.idNavURL : targetEntity.idNavURL;
            targetEntity.nodeOrder = sourceEntity.nodeOrder != null ? sourceEntity.nodeOrder : targetEntity.nodeOrder;

        }


        internal List<MenuNodeDataView> GetAll(CallContext resultContext)
        {


            List<MenuNodeDataView> list = (from m in dbContext.MenuNodes
                                           join u in dbContext.NavURLs on m.idNavURL equals u.idNavURL
                                           orderby m.nodeOrder
                                           select new MenuNodeDataView
                                           {
                                               idNode = m.idNode,
                                               name = m.name,
                                               parentNode = m.parentNode,
                                               prevNode = m.prevNode,
                                               nextNode = m.nextNode,
                                               idNavURL = m.idNavURL,
                                               nodeOrder = m.nodeOrder,
                                               type = m.type,
                                               URL = u.URL,
                                               Code = u.code

                                           }).ToList();
            return list;
        }

        public string CreateManuNodeXML(CallContext resultContext)
        {


            this.listChapters = GetAll(resultContext);

            XmlDeclaration xmlDeclaration = this.xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");

            this.xmlDoc.InsertBefore(xmlDeclaration, this.xmlDoc.DocumentElement);

            XmlElement xmlElRootNode = this.xmlDoc.CreateElement("NodeName");

            xmlElRootNode.SetAttribute("NodeID", "0");
            xmlElRootNode.SetAttribute("NodeName", "Основно меню");

            this.xmlDoc.AppendChild(xmlElRootNode);

            this.depth = -1;

            GetTreeNodeChildren(xmlElRootNode, 0);



            return this.xmlDoc.OuterXml;
        }

        private void GetTreeNodeChildren(XmlElement xmlParentNode, int parentNodeID)
        {
            var listChildChapters = (from c in this.listChapters
                                     where c.parentNode == parentNodeID
                                     orderby c.idNode ascending
                                     select c).ToList<MenuNodeDataView>();

            foreach (MenuNodeDataView chapter in listChildChapters)
            {
                XmlElement xmlChildNode = this.xmlDoc.CreateElement("Node");

                xmlChildNode = FillXmlNodeData(xmlChildNode, chapter);

                GetTreeNodeChildren(xmlChildNode, chapter.idNode);

                if (!xmlParentNode.HasChildNodes)
                {
                    xmlParentNode.AppendChild(xmlChildNode);
                }
                else
                {
                    if (chapter.prevNode.Value == 0 && chapter.nextNode.Value == 0)
                    {
                        xmlParentNode.AppendChild(xmlChildNode);
                    }

                    if (chapter.prevNode.Value == 0 && chapter.nextNode.Value != 0)
                    {
                        xmlParentNode.PrependChild(xmlChildNode);
                    }

                    if (chapter.prevNode.Value != 0 && chapter.nextNode.Value == 0)
                    {
                        xmlParentNode.InsertAfter(xmlChildNode, xmlParentNode.LastChild);
                    }

                    if (chapter.prevNode.Value != 0 && chapter.nextNode.Value != 0)
                    {
                        bool foundNode = false;
                        foreach (XmlElement xmlElement in xmlParentNode.ChildNodes)
                        {
                            if (chapter.prevNode.Value.ToString() == xmlElement.GetAttribute("NodeID"))
                            {
                                foundNode = true;
                                xmlParentNode.InsertAfter(xmlChildNode, xmlElement);
                                break;
                            }
                            if (chapter.nextNode.Value.ToString() == xmlElement.GetAttribute("NodeID"))
                            {
                                foundNode = true;
                                xmlParentNode.InsertBefore(xmlChildNode, xmlElement);
                                break;
                            }
                        }
                        if (!foundNode)
                        {
                            xmlParentNode.AppendChild(xmlChildNode);
                        }
                    }
                }
            }
        }

        private XmlElement FillXmlNodeData(XmlElement xmlNode, MenuNodeDataView node)
        {
            xmlNode.SetAttribute("NodeID", node.idNode.ToString());
            xmlNode.SetAttribute("NodeName", node.name);

            return xmlNode;
        }

        public List<int> GetAllRoleMenuNodeByRoleId(CallContext callContext, int roleID)
        {
            List<int> nodes = this.dbContext.RoleMenuNodes.Where(r => r.idRole == roleID).Select(r => r.idNode).ToList();
            return nodes;
        }
        public List<MenuNodeDataView> GetAllRoleMenuNodeByRoleId(int roleID)
        {
            List<MenuNodeDataView> nodes = (from r in dbContext.RoleMenuNodes
                                            where r.idRole == roleID
                                            join m in dbContext.MenuNodes on r.idNode equals m.idNode
                                            join u in dbContext.NavURLs on m.idNavURL equals u.idNavURL
                                            orderby m.nodeOrder
                                            select new MenuNodeDataView
                                            {
                                                idNode = m.idNode,
                                                name = m.name,
                                                parentNode = m.parentNode,
                                                prevNode = m.prevNode,
                                                nextNode = m.nextNode,
                                                idNavURL = m.idNavURL,
                                                nodeOrder = m.nodeOrder,
                                                type = m.type,
                                                URL = u.URL,
                                                Code = u.code,
                                                QueryParams = u.QueryParams
                                            }).ToList();

            return nodes;
        }

        internal List<MenuNodeDataView> GetAllRoleMenuNodeByAllRoles(List<Role> roles)
        {
            List<MenuNodeDataView> nodes = (from r in roles
                                            join rm in dbContext.RoleMenuNodes on r.idRole equals rm.idRole

                                            join m in dbContext.MenuNodes on rm.idNode equals m.idNode
                                            join u in dbContext.NavURLs on m.idNavURL equals u.idNavURL
                                            orderby m.nodeOrder
                                            select new MenuNodeDataView
                                            {
                                                idNode = m.idNode,
                                                name = m.name,
                                                parentNode = m.parentNode,
                                                prevNode = m.prevNode,
                                                nextNode = m.nextNode,
                                                idNavURL = m.idNavURL,
                                                nodeOrder = m.nodeOrder,
                                                type = m.type,
                                                URL = u.URL,
                                                Code = u.code,
                                                QueryParams = u.QueryParams,
                                                IdRole = r.idRole,
                                            }).ToList();

            return nodes;
        }


        internal CallContext RemoveMenuNode(int nodeID, CallContext resultContext)
        {
            MenuNode node = dbContext.MenuNodes.FirstOrDefault(n => n.idNode == nodeID);
            resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
            resultContext.Message = BaseHelper.GetCaptionString("Entity_Not_Found");
            if (node != null)
            {
                List<RoleMenuNode> nodeForeignKeys = dbContext.RoleMenuNodes.Where(n => n.idNode == nodeID).ToList();
                if (nodeForeignKeys.Count > 0)
                {
                    for (int i = 0; i < nodeForeignKeys.Count; i++)
                    {
                        dbContext.RoleMenuNodes.DeleteObject(nodeForeignKeys[i]);
                    }

                }

                dbContext.MenuNodes.DeleteObject(node);
                dbContext.SaveChanges();
                resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                resultContext.Message = BaseHelper.GetCaptionString("Entity_Deletetion_Successful");

            }
            return resultContext;
        }

        public string GetMenuNodeFullPath(int idChildNode, string nodeName)
        {
            string resultPath = string.Empty;

            var chapter = (from c in dbContext.MenuNodes
                           where c.idNode == idChildNode
                           select c).FirstOrDefault();



            if (chapter.parentNode != 0)
            {
                int idNode = Int32.Parse(chapter.parentNode.ToString());
                GetMenuNodeFullPath(idNode, resultPath);


            }

            if (nodeName == string.Empty)
            {
                this.path += chapter.name + ">>";
            }

            return this.path.Substring(0, this.path.Length - 2);

        }




    }
}