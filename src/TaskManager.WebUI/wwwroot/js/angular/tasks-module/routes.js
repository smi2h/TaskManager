(function (angular) {
    "use strict";

    angular.module("tasks-module")
        .constant("TasksRoutes", {
            displayTask: "displayTask",
            addTask: "addTask"
        })
        .config(["$stateProvider", "$urlRouterProvider", "TasksRoutes",
            function ($stateProvider, $urlRouterProvider, tasksRoutes) {
                $urlRouterProvider.otherwise(tasksRoutes.displayTask);

                $stateProvider
                    .state(tasksRoutes.displayTask, {
                        url: "/" + tasksRoutes.monitoringInfo,
                        templateUrl: "DisplayTabTemplate",
                        breadcrumbName: "Просмотр"
                    })

                    .state(tasksRoutes.monitoringDownloads, {
                        url: "/" + tasksRoutes.addTask,
                        templateUrl: "AddTabTemplate",
                        breadcrumbName: "Дабавление"
                    });
            }]);
})(angular);