/// <binding BeforeBuild='min' Clean='clean' />
"use strict";

// Переменные модулей Gulp
var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssnano = require("gulp-cssnano"),
    rename = require("gulp-rename"),
    uglify = require("gulp-uglify"),
    merge = require("merge-stream");


// Переменные путей до файлов

var paths = {
    webroot: "./wwwroot/",
    theme: "./node_modules/admin-lte/dist/"
};

var deps = {
    "admin-lte": {
        "/plugins/**/*": ""
    }
};

var plugs = {
    "magnific-popup": {
        "/dist/**/*": ""
    },

    "jquery-easing": {
        "/dist/**/*": ""
    }
}

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";

paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";

paths.libs = paths.webroot + "plugins";

paths.landingcss = paths.webroot + "css/landing.css";
paths.landingjs = paths.webroot + "js/landing.js";

paths.concatCssDest = paths.webroot + "css/main.min.css";
paths.concatJsDest = paths.webroot + "js/main.min.js";


paths.themecss = paths.theme + "css/adminlte.css";
paths.themejs = paths.theme + "js/adminlte.js";

// Задачи удаления ненужный файлов

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean:plugins", function (cb) {
    rimraf(paths.libs, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

// Задачи минификации файлов

gulp.task("landing:minjs", function () {
    return gulp.src([paths.landingjs])
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest("./wwwroot/js"));
});

gulp.task("landing:mincss", function () {
    return gulp.src([paths.landingcss])
        .pipe(cssnano())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest("./wwwroot/css"));
});

gulp.task("main:minjs", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});


gulp.task("main:mincss", function () {
    return gulp.src([paths.css, "!" + paths.minCss, "!" + paths.landingcss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssnano())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["main:minjs", "main:mincss" , "landing:minjs", "landing:mincss"]);


// Копирование плагинов

gulp.task("copy:plugins", ["theme:copyplugins"], function () {
    var streams = [];

    for (var prop in plugs) {
        for (var itemProp in plugs[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("wwwroot/plugins/" + prop + "/" + plugs[prop][itemProp])));
        }
    }

    return merge(streams);
})

// Задачи копирования темы

gulp.task("theme:copycss", function () {
    return gulp.src([paths.themecss])
        .pipe(gulp.dest("./wwwroot/css"));
});

gulp.task("theme:copyjs", function () {
    return gulp.src([paths.themejs])
        .pipe(gulp.dest("./wwwroot/js"));
});

gulp.task("theme:copyplugins", function () {

    var streams = [];

    for (var prop in deps) {
        for (var itemProp in deps[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("wwwroot/plugins/" + deps[prop][itemProp])));
        }
    }

    return merge(streams);

});
