$(function () {
    const reportService = bK2T.bankDataReporting.reports.report;

    Search();

    $('#ReportSearchButton').click(function () {
        Search();
    });

    function Search() {
        abp.ui.setBusy();
        var params = {
            reportId: reportId,
            reportType: reportType,
            reportDate: $("#ViewModel_ReportDate").val(),
            departmentId: $("#comboboxDepartments").val() ? $("#comboboxDepartments").val() : null,
            userId: $("#comboboxUsers").val() ? $("#comboboxUsers").val() : null,
            customParams: GetJsonQuery()
        }
        reportService.getColumnsByReportId(params)
            .then(function (result) {
                abp.ui.clearBusy();
                $('#ReportDataTable').DataTable({
                    processing: true,
                    serverSide: true,
                    paging: true,
                    searching: false,
                    autoWidth: true,
                    fixedColumns: true,
                    fixedHeader: true,
                    bLengthChange: false,
                    scrollCollapse: true,
                    scrollX: true,
                    scrollY: '500px',
                    order: [],
                    lengthMenu: [[200, 500, 1000], [200, 500, 1000]],
                    bDestroy: true,
                    ajax: abp.libs.datatables.createAjax(reportService.getReportItemsByReportId, function () {
                        return params;
                    }),
                    columns: result,
                    dom: 'Bfrtip',
                    buttons: [
                        {
                            extend: 'csv',
                            text: '<span class="fa fa-file-csv"></span> CSV',
                            className: 'btn btn-success',
                            filename: `${reportName}`,
                            bom: true
                        },
                    ]
                })
            });
    }

    function GetJsonQuery() {
        const inputs = document.getElementById('Queryable').getElementsByTagName('input')
        const arrObject = {};
        for (const input of inputs) {
            if (input.value) {
                arrObject[input.id] = input.value;
            }
        }
        return arrObject;
    }
});