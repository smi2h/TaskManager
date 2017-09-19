"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    rename = require("gulp-rename"), 
    templateCache = require("gulp-angular-templatecache"),
    ngmin = require("gulp-ngmin"),
    gutil = require("gulp-util"),
    less = require("gulp-less"),
    htmlmin = require("gulp-htmlmin"),
    merge = require("merge-stream"),
    del = require("del"),
    bundleconfig = require("./bundleconfig.json");

var configPath = "../../config/";

var regex = {
    css: /\.css$/,
    html: /\.(html|htm)$/,
    js: /\.js$/
}; 

var paths = {
    webroot: "./wwwroot/"
};
  
paths.js = [
    paths.webroot + "js/angular/*-module/bundles/*.js",
    paths.webroot + "js/*.js"
];
paths.minJs = paths.webroot + "js/**/*.min.js";

paths.css = [
    paths.webroot + "css/*.css",
    paths.webroot + "css/homer/homer.css"
];
paths.minCss = paths.webroot + "css/**/*.min.css";


gulp.task("generateFileWithTemplates", function () {
    return gulp.src(["wwwroot/js/**/*.html"])
        .pipe(htmlmin({ collapseWhitespace: true, minifyCSS: true, minifyJS: true }))
        .pipe(templateCache("js/templates.js", { root: "/js", module: "templates", standalone: true }))
        .pipe(gulp.dest("wwwroot"));
});

gulp.task("bundle:js", function () {
    var tasks = getBundles(regex.js).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(gulp.dest("."));
    });
    return merge(tasks);
});

gulp.task("min:js", function () {
    var minifyJsFiles = paths.js.concat("!" + paths.minJs);

    paths.excludeJs.forEach(function (i) {
        minifyJsFiles.push("!" + paths.webroot + i);
    });

    gulp.src(minifyJsFiles, { base: "." })
        .pipe(ngmin().on("error", gutil.log))
        .pipe(uglify().on("error", gutil.log))
        .pipe(rename(function (path) {
            path.extname = ".min.js";
        }))
        .pipe(gulp.dest("."));
});


gulp.task("less", function () {
    gulp.src(["wwwroot/css/custom.less", "wwwroot/css/selectize.default.less"])
        .pipe(less())
        .pipe(gulp.dest("wwwroot/css/"));

    gulp.src("wwwroot/css/homer/homer.less")
        .pipe(less())
        .pipe(gulp.dest("wwwroot/css/homer"));
});

gulp.task("min:css", function () {
    var gulpCssPaths = paths.css.concat(["!" + paths.minCss]);

    gulp.src(gulpCssPaths, { base: "." })
        .pipe(cssmin())
        .pipe(rename({ suffix: ".min" }))
        .pipe(gulp.dest("."));
});

function getBundles(regexPattern) {
    return bundleconfig.filter(function (bundle) {
        return regexPattern.test(bundle.outputFileName);
    });
} 