$(function () {
    const l = abp.localization.getResource('BankDataReporting');
    const service = bK2T.bankDataReporting.reportTemplates.reportTemplate;
    const createModal = new abp.ModalManager(`${abp.appPath}ReportTemplates/Reports/CreateModal`);
    const editModal = new abp.ModalManager(`${abp.appPath}ReportTemplates/Reports/EditModal`);

    const dataTable = $('#ReportTemplatesTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[0, "asc"]],
        ajax: abp.libs.datatables.createAjax(service.getListReport, function () { return $('#ReportType').val() }),
        columnDefs: [
            {
                title: l('Reports:Name'),
                data: "name",
                width: "30%"
            },

            {
                title: l('Actions'),
                width: "20%",
                rowAction: {
                    items:
                        [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('BankDataReporting.ReportTemplates.Update'),
                                action: function (data) {
                                    editModal.open({ reportType: $('#ReportType').val(),id: data.record.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('BankDataReporting.ReportTemplates.Delete'),
                                confirmMessage: function (data) {
                                    return l('Common:DeletionConfirmationMessage', data.record.id);
                                },
                                action: function (data) {
                                    service.deleteReport($('#ReportType').val(), data.record.id)
                                        .then(function () {
                                            abp.notify.info(l('Common:SuccessfullyDeleted'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                }
            }
        ],
        "fnDrawCallback": function (oSettings) {
            if ($('#ReportTemplatesTable').DataTable().page.info().pages < 2) {
                $('.dataTable_footer').hide();
            } else {
                $('.dataTable_footer').show();
            }
        }
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#ReportType').on('change', function () {
        dataTable.ajax.reload();
    });

    $('#NewReportTemplateButton').click(function (e) {
        e.preventDefault();
        createModal.open({ reportType: $('#ReportType').val()});
    })
})