function showForm() {

  if (Xrm.Page.ui.getFormType()==2) 
  {
     var lblForm;

     // get the value picklist field

     var relType = Xrm.Page.getAttribute("new_type").getValue();
     //alert(relType);

     switch(relType) {
       case 1:
        lblForm = "Hotel";
        break;
       case 2:
        lblForm = "Business center";
        break;
       case 3:
        lblForm = "BigBox";
        break;
       case 4:
        lblForm = "Information";
        break;
       case 5:
        lblForm = "Mall";
        break;
       case 6:
        lblForm = "Information";
        break;
       case 7:
        lblForm = "Residential Building";
        break;
       default: lblForm = "Information";
         break;
     }
     //alert(lblForm);

     if (Xrm.Page.ui.formSelector.getCurrentItem().getLabel() != lblForm) {

       var items = Xrm.Page.ui.formSelector.items.get();

       for (var i in items) {

           var item = items[i];

           var itemId = item.getId();

           var itemLabel = item.getLabel()

           if (itemLabel == lblForm) {

               //navigate to the form

               item.navigate();

           } //endif

       } //end for

   } //endif
  }
} 