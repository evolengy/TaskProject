$(document).ready(function () { 
    $.ajaxSetup({ cache: false });

    $(function () {
        $("#sortable").sortable({
            placeholder: "ui-state-highlight"
        });
        $("#sortable").disableSelection();
    });

// Popovers Bootstrap

    $(function () {
        $('[data-toggle="popover"]').popover()
    })

    $(".js-health").popover({
        container: 'body',
        placement: "right",
        trigger: "hover",
        title: "Здоровье персонажа",
        content: "Ваше здоровье обозначает количество оставшихся дней до даты смерти, которая была установлена в соответствии" +
            " с указанными ответами на вопросы анкеты."

    });

    $(".js-karma").popover({
        container: 'body',
        placement: "right",
        trigger: "hover",
        title: "Карма персонажа",
        content: "Карма начисляется в зависимости от ваших поступков, которые вы отмечаете ежедневно."

    });

    $(".js-datedeath").popover({
        container: 'body',
        placement: "right",
        trigger: "hover",
        title: "Предположительная дата смерти персонажа",
        content: "Предположительная дата смерти была выведена на основе ваших ответов на вопросы анкеты."
    });

    $(".js-exp").popover({
        container: 'body',
        placement: "right",
        trigger: "hover",
        title: "Опыт персонажа",
        content: "Опыт персонажу дается за выполнение задач."

    });

    $(".js-mood").popover({
        container: 'body',
        placement: "right",
        trigger: "hover",
        title: "Настроение персонажа",
        content: "Приложение позволяет учитывать ваше ежедневное настроение. Можно будет посмотреть статистику по дням, месяцам и годам - " +
            "какие из них не очень задались."

    });


//CRUD Modal Form

    var link;

    // Rewards
    $(".js-addreward").on("click", function (e) {
        e.preventDefault();
        $(".sw_title").text($(this).attr("data_dialog_title"));
        $(".sw_modal_result").load(this.href);
        $(".sw_modal").modal("show", function () {
        });

        link = "/Rewards/AddReward";
    });


    $(".js-addkarma").on("click", function (e) {
        e.preventDefault();
        $(".sw_title").text($(this).attr("data_dialog_title"));
        $(".sw_modal_result").load(this.href);
        $(".sw_modal").modal("show", function () {
        });

        link = "/Karma/Add";
    });

    $(".js-editreward").on("click", function (e) {

        e.preventDefault();
        $(".sw_title").text($(this).attr("data_dialog_title"));
        $(".sw_modal_result").load(this.href);
        $(".sw_modal").modal("show", function () {
        });

        link = "/Rewards/EditReward";
    });

    // Catalogs
    $(".js-addcatalog").on("click", function (e) {
        e.preventDefault();
        $(".sw_title").text($(this).attr("data_dialog_title"));
        $(".sw_modal_result").load(this.href);
        $(".sw_modal").modal("show", function () {
        });

        link = "/Goals/AddCatalog";
    });

    $(".js-editcatalog").on("click", function (e) {

        e.preventDefault();
        $(".sw_title").text($(this).attr("data_dialog_title"));
        $(".sw_modal_result").load(this.href);
        $(".sw_modal").modal("show", function () {
        });

        link = "/Goals/EditCatalog";
    });

    //NickName
    $(".js-addnickname").on("click", function (e) {
        e.preventDefault();
        $(".sw_title").text($(this).attr("data_dialog_title"));
        $(".sw_modal_result").load(this.href);
        $(".sw_modal").modal("show", function () {
        });

        link = "/Home/AddNickName";
    });

    $(".js-editnickname").on("click", function (e) {

        e.preventDefault();
        $(".sw_title").text($(this).attr("data_dialog_title"));
        $(".sw_modal_result").load(this.href);
        $(".sw_modal").modal("show", function () {
        });

        link = "/Home/EditNickName";
    });

    // Goals
    $(".js-addgoal").on("click", function (e) {
        e.preventDefault();
        $(".sw_title").text($(this).attr("data_dialog_title"));
        $(".sw_modal_result").load(this.href);
        $(".sw_modal").modal("show", function () {
        });

        link = "/Goals/AddGoal";
    });

    $(".js-editgoal").on("click", function (e) {

        e.preventDefault();
        $(".sw_title").text($(this).attr("data_dialog_title"));
        $(".sw_modal_result").load(this.href);
        $(".sw_modal").modal("show", function () {
        });

        link = "/Goals/EditGoal";
    });

    $(".js-completegoal").on("click", function (e) {
        e.preventDefault();
        $(".sw_title").text($(this).attr("data_dialog_title"));
        $(".sw_modal_result").load(this.href);
        $(".sw_modal").modal("show", function () {
        });

        link = "/Goals/CompleteGoal";
    });

    $(".js-restoregoal").on("click", function (e) {
        e.preventDefault();
        $(".sw_title").text($(this).attr("data_dialog_title"));
        $(".sw_modal_result").load(this.href);
        $(".sw_modal").modal("show", function () {
        });

        link = "/Goals/RestoreGoal";
    });
    // Skills

    $(".js-addskill").on("click", function (e) {
        e.preventDefault();
        $(".sw_title").text($(this).attr("data_dialog_title"));
        $(".sw_modal_result").load(this.href);
        $(".sw_modal").modal("show", function () {
        });

        link = "/Skills/AddSkill";
    });

    $(".js-editskill").on("click", function (e) {

        e.preventDefault();
        $(".sw_title").text($(this).attr("data_dialog_title"));
        $(".sw_modal_result").load(this.href);
        $(".sw_modal").modal("show", function () {
        });

        link = "/Skills/EditSkill";
    });


    // Submit Form

    $(".sw_modal_submit").on("click", function (e) {
        e.preventDefault();
        var form = $('.form-modal').serialize();
        $.ajax({
            url: link,
            type: "POST",
            data: $('.form-modal').serialize(),
            datatype: "json",
            success: function (result) {
                $(".sw_modal_result").html(result);
            }
        });
    });

});

function keydownenter(event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
};

//Delete Element without load

function deletecatalog(catalogid) {
    $.ajax({
        url: '/Goals/DeleteCatalog',
        type: "POST",
        data: { id: catalogid },
        success: function () {
            $("#catalog-" + catalogid).remove();
        }
    });
};

function deletegoal(goalid) {
    $.ajax({
        url: '/Goals/DeleteGoal',
        type: "POST",
        data: { id: goalid },
        success: function () {
            $("#goal-" + goalid).remove();
        }
    });
};

function deleteskill(skillid) {
    $.ajax({
        url: '/Skills/DeleteSkill',
        type: "POST",
        data: { id: skillid },
        success: function () {
            $("#skill-" + skillid).remove();
        }
    });
};

function deletereward(rewardid) {
    $.ajax({
        url: '/Rewards/DeleteReward',
        type: "POST",
        data: { id: rewardid },
        success: function () {
            $("#reward-" + rewardid).remove();
        }
    });
};

//$(document).ready(function () {
//    var dead = $(".sw_dead").val();
//    if (dead === "True") {
//        $.ajax({
//            url: '@Url.Action("Dead","Home")',
//            success: function (data) {
//                $('.sw_modal_result').html(data);
//                document.getElementById('modal-header').style.display = "none";
//                $('.sw_modal_goals').modal({
//                    backdrop: 'static',
//                    keyboard: false
//                });
//            }
//        })
//    }
//});
