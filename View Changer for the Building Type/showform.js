function showForm() {

//if the form is update form

if (Xrm.Page.ui.getFormType()==2)

   // variable to store the name of the form

   var lblForm;

   // get the value picklist field

   var relType = Xrm.Page.getAttribute("new_premisetype").getValue();
   alert(relType);

   // switch statement to assign the form to the picklist value

   //change the switch statement based on the forms numbers and picklist values

   switch (relType) {

       case 1:

           lblForm = "Stores";

           break;

       case 2:

           lblForm = "Information";

           break;
     /*  case 3:

           lblForm = "Offices";

           break;

       case 4:

           lblForm = "Garages";

           break;
       case 5:

           lblForm = "Warehouse";

           break;

       case 6:

           lblForm = "Industrial premises";

           break;*/

       default:

           lblForm = "Information";
           alert(lblForm);

   }

   //check if the current form is form need to be displayed based on the value

 /*  if (Xrm.Page.ui.formSelector.getCurrentItem().getLabel() != lblForm) {

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

   } //endif*/

}//endif

} //end function
