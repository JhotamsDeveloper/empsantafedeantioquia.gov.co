const gulp = require('gulp'),
    sourcemaps = require('gulp-sourcemaps'),
    sass = require('gulp-sass'),
    minify = require('gulp-minify-css'),
    uglify = require('gulp-uglify'),
    concat = require('gulp-concat'),
    htmlreplace = require('gulp-html-replace'),
    autoprefixer = require('gulp-autoprefixer'),
    rename = require('gulp-rename'),
    browserSync = require('browser-sync').create(),
    src = 'public/src', // 작업파일
    dist = 'public/dist', // 배포파일
    scssOptions = {
        outputStyle: 'expanded', // 컴파일 결과 코드스타일 지정 (nested, expanded, compact, compressed)
        indentType: 'tab', // css 들여쓰기 타입 (space, tab)
        indentWidth: 1, // outputStyle이 nested, expanded인 경우 사용. 들여쓰기의 갯수 (default: 2)
        precision: 6, // 컴파일 된 css의 소수점 자리수 (default: 5)
        sourceComments: true // 컴파일 된 css에 원본소스의 위치와 줄 수 주석 표시
    };

// html extract
gulp.task('html:copy', () => {
    return gulp
        .src(src + '/**/*.html')
        .pipe(
            htmlreplace({
                css: './css/common.min.css',
                jquery: './js/jquery.js',
                slider: './js/jquery.jdSlider-latest.min.js'
            }) // css, js 인크루드 병합
        )
        .pipe(gulp.dest(dist))
        .pipe(browserSync.reload({ stream: true })); // browserSync로 브라우저에 반영
});

// scss compile
gulp.task('scss:compile', () => {
    return gulp
        .src(src + '/scss/*.scss') // scss 파일 설정
        .pipe(sourcemaps.init()) // 소스맵 초기화
        .pipe(sass(scssOptions).on('error', sass.logError)) // scss 함수에 옵션값을 설정, scss 작성시 watch가 멈추지 않도록 logError 설정
        .pipe(
            autoprefixer({
                browsers: ['last 2 versions'],
                cascade: false
            })
        )
        .pipe(sourcemaps.write()) // 위에서 생성한 소스맵 사용
        .pipe(gulp.dest(src + '/css')); // css 컴파일 위치 설정
});

// css minify
gulp.task('css:minify', ['scss:compile'], () => {
    return gulp
        .src([src + '/css/reset.css', src + '/css/jquery.jdSlider.css']) // src 폴더 모든 css 파일을
        .pipe(concat('common.min.css'))
        .pipe(minify()) // @import를 분석 후 하나의 파일로 병합하고 minify
        .pipe(gulp.dest(dist + '/css'))
        .pipe(browserSync.reload({ stream: true }));
});

// jquery load
gulp.task('js:jquery', () => {
    return gulp
        .src('./node_modules/jquery/dist/jquery.js')
        .pipe(rename('jquery.js'))
        .pipe(gulp.dest(dist + '/js'));
});

// js minify
gulp.task('js:uglify', ['js:jquery'], () => {
    return gulp
        .src(src + '/js/jquery.jdSlider-latest.js')
        .pipe(rename('jquery.jdSlider-latest.min.js')) // jquery.jdSlider-latest.min.js 라는 파일명으로
        .pipe(uglify())
        .pipe(gulp.dest(dist + '/js'))
        .pipe(browserSync.reload({ stream: true }));
});

// dist 폴더 기준으로 웹서버 실행
gulp.task('server', ['html:copy', 'css:minify', 'js:uglify'], () => {
    return browserSync.init({
        server: {
            baseDir: dist
        }
    });
});

// 파일 변경 감지
gulp.task('watch', () => {
    gulp.watch(src + '/**/*.html', ['html:copy']);
    gulp.watch(src + '/scss/*.scss', ['scss:compile']);
    gulp.watch(src + '/css/*.css', ['css:minify']);
    gulp.watch(src + '/js/*.js', ['js:uglify']);
});

// 실행
gulp.task('default', ['server', 'watch']);
