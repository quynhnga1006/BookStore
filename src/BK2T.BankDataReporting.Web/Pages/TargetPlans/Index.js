$(function () {
    const l = abp.localization.getResource('BankDataReporting');
    const service = bK2T.bankDataReporting.departments.department;
    const editModal = new abp.ModalManager(`${abp.appPath}TargetPlans/EditModal`);
    var dataTable;

    Search();

    $('#ReportSearchButton').click(function () {
        Search();
    });

    function Search() {
        var params = {
            departmentId: $("#comboboxDepartments").val(),
            planType: $("#comboboxPlanType").val(),
            year: $("#comboboxYears").val()
        }
        dataTable = $('#TargetPlanTable').DataTable(abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: true,
            searching: false,
            autoWidth: false,
            scrollCollapse: true,
            bDestroy: true,
            order: [[0, "asc"]],
            ajax: abp.libs.datatables.createAjax(service.getTargetPlans, function () {
                return params;
            }),
            columnDefs: [
                {
                    title: l('TargetPlans:PlanType'),
                    data: function (data) {
                        return l('TargetPlans:PlanType:' + data.planType);
                    },
                    width: "20%"
                },{
                    title: l('TargetPlans:UnitMeasure'),
                    data: function (data) {
                        return l('TargetPlans:UnitMeasure:' + data.unitMeasure);
                    },
                    width: "5%"
                },
                {
                    title: l('TargetPlans:YearTarget'),
                    data: "yearTarget",
                    width: "5%"
                },
                {
                    title: l('TargetPlans:MonthTarget:00003','1'),
                    data: "monthTarget.0",
                    width: "5%"
                },
                {
                    title: l('TargetPlans:MonthTarget:00003','2'),
                    data: "monthTarget.1",
                    width: "5%"
                },
                {
                    title: l('TargetPlans:MonthTarget:00003','3'),
                    data: "monthTarget.2",
                    width: "5%"
                },
                {
                    title: l('TargetPlans:MonthTarget:00003','4'),
                    data: "monthTarget.3",
                    width: "5%"
                },
                {
                    title: l('TargetPlans:MonthTarget:00003','5'),
                    data: "monthTarget.4",
                    width: "5%"
                },
                {
                    title: l('TargetPlans:MonthTarget:00003','6'),
                    data: "monthTarget.5",
                    width: "5%"
                },
                {
                    title: l('TargetPlans:MonthTarget:00003','7'),
                    data: "monthTarget.6",
                    width: "5%"
                },
                {
                    title: l('TargetPlans:MonthTarget:00003','8'),
                    data: "monthTarget.7",
                    width: "5%"
                },
                {
                    title: l('TargetPlans:MonthTarget:00003','9'),
                    data: "monthTarget.8",
                    width: "5%"
                },
                {
                    title: l('TargetPlans:MonthTarget:00003','10'),
                    data: "monthTarget.9",
                    width: "5%"
                },
                {
                    title: l('TargetPlans:MonthTarget:00003','11'),
                    data: "monthTarget.10",
                    width: "5%"
                },
                {
                    title: l('TargetPlans:MonthTarget:00003','12'),
                    data: "monthTarget.11",
                    width: "5%"
                },
                {
                    title: l('Actions'),
                    data: { planType: "planType" }, render: function (data) {
                        var htmlRender = '';
                        if (abp.auth.isGranted('BankDataReporting.Departments.Update')){
                            if (data.planType != 0 && data.planType != 5 && data.planType != 10 && data.planType != 16 && data.planType != 22 && data.planType != 25 && data.planType != 31) {
                                htmlRender += '<button type="button" class="btn btn-primary abp-action-button" _planType="' + data.planType + '" _type="edit">' + l("Edit") + '</button > ';
                            }
                        }                            
                        return htmlRender;
                    },
                    width: "20%",
                },
            ],
            "fnDrawCallback": function (oSettings) {
                if ($('#TargetPlanTable').DataTable().page.info().pages < 2) {
                    $('.dataTable_footer').hide();
                } else {
                    $('.dataTable_footer').show();
                }
            }
        }));
        editModal.onResult(function () {
            dataTable.ajax.reload();
        });
    }

    $('#TargetPlanTable tbody').on('click', 'button', function () {
        var id = $(this).attr("_id");
        var planType = $(this).attr("_planType");
        if ($(this).attr("_type") === "edit") {
            editModal.open({
                departmentId: $("#comboboxDepartments").val(),
                planType: planType,
                year: $("#comboboxYears").val()
            });
        }
    });
})