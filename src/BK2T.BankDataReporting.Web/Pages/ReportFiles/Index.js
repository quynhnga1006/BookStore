$(function () {
    const l = abp.localization.getResource('BankDataReporting');
    const service = bK2T.bankDataReporting.reportFiles.reportFile;
    const createModal = new abp.ModalManager(abp.appPath + 'ReportFiles/CreateModal');

    const dataTable = $('#ReportFileTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[0, "asc"]],
        ajax: abp.libs.datatables.createAjax(service.getList),
        columnDefs: [
            {
                title: l('ReportFiles:User'),
                width: "13%",
                data: "createdByUsername",
                render: function (data) {
                    return data;
                }
            },
            {
                title: l('ReportFiles:UploadDate'),
                width: "13%",
                data: "createdDate",
                dataFormat: 'date'
            },
            {
                title: l('ReportFiles:ReportDate'),
                width: "13%",
                data: "reportDate",
                dataFormat: 'date'
            },
            {
                title: l('ReportFiles:ReportType'),
                width: "15%",
                data: "reportTypeCode",
                render: function (code) {
                    return l(code);
                }
            },
            {
                title: l('ReportFiles:Status'),
                width: "13%",
                data: "reportStatusCode",
                render: function (code) {
                    return l(code);
                }
            },
            {
                orderable: false,
                title: l('ReportFiles:ReportFile'),
                width: "20%",
                data: "fileData",
                render: function (data) {
                    return `<a href="/api/app/file/download?fileName=${data}">${data}`;
                }
            },
            {
                title: l('Actions'),
                width: "13%",
                rowAction: {
                    items:
                        [
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('BankDataReporting.ReportFiles.Delete'),
                                confirmMessage: function (data) {
                                    return l('Common:DeletionConfirmationMessage', data.record.id);
                                },
                                action: function (data) {
                                    service.delete(data.record.id)
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
            if ($('#ReportFileTable').DataTable().page.info().pages < 2) {
                $('.dataTable_footer').hide();
            } else {
                $('.dataTable_footer').show();
            }
        }
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewReportFilesButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    })
})