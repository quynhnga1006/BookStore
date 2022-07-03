$(function () {
    const l = abp.localization.getResource('BankDataReporting');
    const service = bK2T.bankDataReporting.departments.department;
    const createModal = new abp.ModalManager(`${abp.appPath}Departments/CreateModal`);
    const editModal = new abp.ModalManager(`${abp.appPath}Departments/EditModal`);

    const dataTable = $('#DepartmentTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        serverSide: false,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[0, "asc"]],
        ajax: abp.libs.datatables.createAjax(service.getList),
        columnDefs: [
            {
                title: l('Departments:Code'),
                data: "code",
                width: "15%"
            },
            {
                title: l('Departments:Name'),
                data: "name",
                width: "25%"
            },
            {
                title: l('Departments:OldCode'),
                data: "oldCode",
                width: "15%"
            },
            {
                title: l('Departments:CustomerSegments'),
                data: "customerSegments",
                width: "25%"
            },
            {
                title: l('Actions'),
                width: "20%",
                rowAction: {
                    items:
                        [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('BankDataReporting.Departments.Update'),
                                action: function (data) {
                                    editModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('BankDataReporting.Departments.Delete'),
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
            if ($('#DepartmentTable').DataTable().page.info().pages < 2) {
                $('.dataTable_footer').hide();
            } else {
                $('.dataTable_footer').show();
            }
        }
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewDepartmentButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
})