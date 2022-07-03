$(function () {
    const l = abp.localization.getResource('BankDataReporting');
    const service = bK2T.bankDataReporting.departments.department;
    const editModal = new abp.ModalManager(`${abp.appPath}ManualCapitals/EditModal`);
    var dataTable;
    Search();

    $('#ReportSearchButton').click(function () {
        Search();
    });

    function Search() {
        var params = {
            departmentId: $("#comboboxDepartments").val(),
            year: $("#comboboxYears").val()
        }
        dataTable = $('#ManualCapitalTable').DataTable(abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: true,
            searching: false,
            autoWidth: false,
            scrollCollapse: true,
            bDestroy: true,
            order: [[0, "asc"]],
            ajax: abp.libs.datatables.createAjax(service.getManualCapitals, function () {
                return params;
            }),
            columnDefs: [
                {
                    title: l('ManualCapitals:CustomerType'),
                    data: function (data) {
                        return l('ManualCapitals:CustomerType:' + data.customerType);
                    },
                    width: "20%"
                }, {
                    title: l('TargetPlans:UnitMeasure'),
                    data: function (data) {
                        return l('TargetPlans:UnitMeasure:' + data.unitMeasure);
                    },
                    width: "5%"
                },
                {
                    title: l('ManualCapitals:MonthCapital:00004', '1'),
                    data: "monthsCapital.0",
                    width: "5%"
                },
                {
                    title: l('ManualCapitals:MonthCapital:00004', '2'),
                    data: "monthsCapital.1",
                    width: "5%"
                },
                {
                    title: l('ManualCapitals:MonthCapital:00004', '3'),
                    data: "monthsCapital.2",
                    width: "5%"
                },
                {
                    title: l('ManualCapitals:MonthCapital:00004', '4'),
                    data: "monthsCapital.3",
                    width: "5%"
                },
                {
                    title: l('ManualCapitals:MonthCapital:00004', '5'),
                    data: "monthsCapital.4",
                    width: "5%"
                },
                {
                    title: l('ManualCapitals:MonthCapital:00004', '6'),
                    data: "monthsCapital.5",
                    width: "5%"
                },
                {
                    title: l('ManualCapitals:MonthCapital:00004', '7'),
                    data: "monthsCapital.6",
                    width: "5%"
                },
                {
                    title: l('ManualCapitals:MonthCapital:00004', '8'),
                    data: "monthsCapital.7",
                    width: "5%"
                },
                {
                    title: l('ManualCapitals:MonthCapital:00004', '9'),
                    data: "monthsCapital.8",
                    width: "5%"
                },
                {
                    title: l('ManualCapitals:MonthCapital:00004', '10'),
                    data: "monthsCapital.9",
                    width: "5%"
                },
                {
                    title: l('ManualCapitals:MonthCapital:00004', '11'),
                    data: "monthsCapital.10",
                    width: "5%"
                },
                {
                    title: l('ManualCapitals:MonthCapital:00004', '12'),
                    data: "monthsCapital.11",
                    width: "5%"
                },
                {
                    title: l('Actions'),
                    data: { customerType: "customerType" }, render: function (data) {
                        var htmlRender = '';
                        if (abp.auth.isGranted('BankDataReporting.Departments.Update')) {
                           htmlRender += '<button type="button" class="btn btn-primary abp-action-button" _customerType="' + data.customerType + '" _type="edit">' + l("Edit") + '</button > ';
                        }
                        return htmlRender;
                    },
                    width: "20%",
                }
            ],
        }));
    };
    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#ManualCapitalTable tbody').on('click', 'button', function () {
    var customerType = $(this).attr("_customerType");
    if ($(this).attr("_type") === "edit") {
        editModal.open({
            departmentId: $("#comboboxDepartments").val(),
            customerType: customerType,
            year: $("#comboboxYears").val()
        });
    }});
})