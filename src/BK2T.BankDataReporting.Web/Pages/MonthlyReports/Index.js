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
        }

        reportService.getColumnsByReportId(params)
            .then(function (result) {
                abp.ui.clearBusy();
                $('#ReportDataTable').DataTable({
                    responsive: true,
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
                    ordering: false,
                    scrollY: '500px',
                    lengthMenu: [[200, 500, 1000], [200, 500, 1000]],
                    bDestroy: true,
                    ajax: abp.libs.datatables.createAjax(reportService.getMonthlyReportItemsByReportId, function () {
                        return params;
                    }),
                    columns: result,
                    dom: 'Bfrtip',
                    buttons: [
                        {
                            extend: 'excel',
                            text: '<span class="fa fa-file-excel-o"></span> Excel',
                            className: 'btn btn-primary',
                            title: null,
                            filename: `${reportId}.xlsx`
                        },
                        {
                            extend: 'csv',
                            text: '<span class="fa fa-file-csv"></span> CSV',
                            className: 'btn btn-success',
                            filename: `${reportId}.csv`,
                            bom: true
                        },
                    ]
                })
            });
    }

});