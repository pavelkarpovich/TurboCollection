function InfinitiySroll(iTable, iAction, iParams) {
    this.table = iTable;        // Reference to the table where data should be added
    this.action = iAction;      // Name of the conrtoller action
    this.params = iParams;      // Additional parameters to pass to the controller
    this.loading = false;       // true if asynchronous loading is in process
    this.skip = 50;

    var tagValues = $("#tags").val();

    this.AddTableLines = function (firstItem) {
        this.loading = true;
        this.params.firstItem = firstItem;
        this.params.tagValues = tagValues;
        // $("#footer").css("display", "block"); // show loading info
        $.ajax({
            type: 'POST',
            url: self.action,
            data: self.params,
            dataType: "html"
        })
            .done(function (result) {
                if (result) {
                    $("#" + self.table).append(result);
                    self.loading = false;
                    self.skip = self.skip + 50;
                }
            })
            .fail(function (xhr, ajaxOptions, thrownError) {
                console.log("Error in AddTableLines:", thrownError);
            })
            .always(function () {
                // $("#footer").css("display", "none"); // hide loading info
            });
    }

    var self = this;
    window.onscroll = function (ev) {
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
            //User is currently at the bottom of the page
            if (!self.loading) {
                //var itemCount = $('#' + self.table + ' tr').length - 1;
                self.AddTableLines(self.skip);
            }
        }
    };
    //this.AddTableLines(0);
}