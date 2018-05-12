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
    libs: "./wwwroot/libs"
};

var deps = {

    "bootstrap": {
        "dist/**/*": ""
    },
    "bootstrap-datepicker": {
        "dist/**/*": ""
    },
    "bootstrap-markdown": {
        "**/*": ""
    },
    "bootstrap-multiselect": {
        "dist/**/*":""
    },
    "bootstrap-progressbar": {
        "**/*":""
    },
    "chartist": {
        "/dist/**/*":""
    },
    "chartist-plugin-axistitle": {
        "/dist/**/*": ""
    },
    "chartist-plugin-tooltip": {
        "/**/*": ""
    },
    "chartist-plugin-legend": {
        "/**/*": ""
    },
    "datatables": {
        "/media/**/*": ""
    },
    "font-awesome": {
        "/**/*":""
    },
    "jquery": {
        "dist/*": ""
    },
    "jquery.maskedinput": {
        "/src/**/*": ""
    },
    "jquery.scrollex": {
        "*.js": ""
    },
    "jquery-slimscroll": {
        "*.js": ""
    },
    "jquery-sparkline": {
        "*.js": ""
    },
    "linearicons": {
        "/dist/**/*": ""
    },
    "markdown": {
        "/lib/**/*": ""
    },
    "metismenu": {
        "/dist/**/*": ""
    },
    "modernizr": {
        "/**/*": ""
    },
    "parsleyjs": {
        "/dist/**/*": ""
    },
    "toastr": {
        "/**/*": ""
    },
    "to-markdown": {
        "/dist/**/*": ""
    }
};
                       
paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";

paths.landingcss = paths.webroot + "css/landing.css";

paths.concatCssDest = paths.webroot + "css/main.min.css";
paths.concatJsDest = paths.webroot + "js/main.min.js";

// Задачи удаления ненужный файлов
gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean:libs", function (cb) {
    rimraf(paths.libs, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

// Задачи минификации файлов
gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:landingcss", function () {
    return gulp.src([paths.landingcss])
        .pipe(cssnano())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest("./wwwroot/css"));
});

gulp.task("min:css", ["min:landingcss"], function () {
    return gulp.src([paths.css, "!" + paths.minCss, "!" + paths.landingcss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssnano())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);


gulp.task("scripts", ["clean:libs"], function () {

    var streams = [];

    for (var prop in deps) {
        for (var itemProp in deps[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("wwwroot/libs/" + prop + "/" + deps[prop][itemProp])));
        }
    }

    return merge(streams);

});