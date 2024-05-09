'use strict';
$(document).ready(function () {
     $('#example-2').Tabledit({

          columns: {

               identifier: [0, 'id'],

               editable: [[1, 'Name'], [2, 'Floor']] 

          }

     });
});

