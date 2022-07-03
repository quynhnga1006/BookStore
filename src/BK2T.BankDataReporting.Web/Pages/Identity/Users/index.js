(function ($) {
    const l = abp.localization.getResource('AbpIdentity');

    const _identityUserAppService = volo.abp.identity.identityUser;
    const _editModal = new abp.ModalManager(
        abp.appPath + 'Identity/Users/EditModal'
    );
    const _createModal = new abp.ModalManager(
        abp.appPath + 'Identity/Users/CreateModal'
    );
    const _permissionsModal = new abp.ModalManager(
        abp.appPath + 'AbpPermissionManagement/PermissionManagementModal'
    );

    let _dataTable = null;

    abp.ui.extensions.entityActions.get('identity.user').addContributor(
        function (actionList) {
            return actionList.addManyTail(
                [
                    {
                        text: l('Edit'),
                        visible: abp.auth.isGranted(
                            'AbpIdentity.Users.Update'
                        ),
                        action: function (data) {
                            _editModal.open({
                                id: data.record.id,
                            });
                        },
                    },
                    {
                        text: l('Permissions'),
                        visible: abp.auth.isGranted(
                            'AbpIdentity.Users.ManagePermissions'
                        ),
                        action: function (data) {
                            _permissionsModal.open({
                                providerName: 'U',
                                providerKey: data.record.id,
                            });
                        },
                    },
                    {
                        text: l('Delete'),
                        visible: abp.auth.isGranted(
                            'AbpIdentity.Users.Delete'
                        ),
                        confirmMessage: function (data) {
                            return l(
                                'UserDeletionConfirmationMessage',
                                data.record.userName
                            );
                        },
                        action: function (data) {
                            _identityUserAppService
                                .delete(data.record.id)
                                .then(function () {
                                    _dataTable.ajax.reload();
                                });
                        },
                    }
                ]
            );
        }
    );

    $(function () {
        _dataTable = $('#TableUserManagement').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                order: [[1, 'asc']],
                processing: true,
                serverSide: true,
                scrollX: true,
                paging: true,
                ajax: abp.libs.datatables.createAjax(
                    _identityUserAppService.getList
                ),
                columnDefs: [
                    {
                        title: l('UserName'),
                        data: 'userName',
                    },
                    {
                        title: l('EmailAddress'),
                        data: 'email',
                    },
                    {
                        title: l('PhoneNumber'),
                        data: 'phoneNumber',
                    },
                    {
                        title: l('Department'),
                        data: 'extraProperties.DepartmentId_Text',
                    },
                    {
                        title: l("Actions"),
                        rowAction: {
                            items: abp.ui.extensions.entityActions.get('identity.user').actions.toArray()
                        }
                    }
                ]
            })
        );

        _createModal.onResult(function () {
            _dataTable.ajax.reload();
        });

        _editModal.onResult(function () {
            _dataTable.ajax.reload();
        });

        $('#NewUserButton').click(function (e) {
            e.preventDefault();
            _createModal.open();
        })
    });
})(jQuery);
