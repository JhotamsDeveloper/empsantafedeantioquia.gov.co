# jdSlider
* IE8~ 호환.
* 반응형에 따른 옵션 변경 가능.

## HTML
* 기본구조

```xml
<div class="jd-slider slider1"> <!-- 옵션값이 다른 경우는 slider1 처럼 클래스를 새로 추가하여 셀렉터로 활용한다 -->
  <div class="slide-inner"> <!-- 필요시에만 감싼다 -->
    <ul class="slide-area">
        <li>1</li>
        <li>2</li>
        <li>3</li>
        <li>이미지4</li>
    </ul>
    <a class="prev" href="#">이전<a>
    <a class="next" href="#">다음</a>
  </div>
  <div class="controller">
    <a class="auto" href="#"></a> <!-- 재생/정지 버튼 -->
    <div class="indicate-area"> <!-- 인디케이트 내부 비어놓을 시 자동으로 생성 -->
      <a href="#">1</a>
      <a href="#">2</a>
      <a href="#">3</a>
      <a href="#">4</a>
    </div>
   </div>
</div>
```

## CSS
* 기본구조

```css
.jd-slider .slide-area {
    width: 100%;
    margin: 0;
    padding: 0;
    -webkit-transform: translate3d(0, 0, 0);
    transform: translate3d(0, 0, 0);
}

/* 슬라이더 플러그인이 활성화 되기 이전에 깨지지 않게 하기 위하여 첫번째 li만 노출 (slideShow:1 을 기본으로 구성) */
.jd-slider .slide-area li {
	display: none;
	float: left;
	width: 100%;
}
.jd-slider .slide-area li:first-child {
	display: block;
}

/* 페이드인 기능 사용 시에 맞는 css 구문 (slide-fade 클래스 활용) */
.jd-slider.fade .slide-area li {
	display: block;
	position: absolute;
	top: 0;
	left: 0;
	opacity: 0;
	-webkit-transform: translateZ(0);
	transform: translateZ(0);
}
.jd-slider.fade .slide-area li:first-child {
	position: static;
	opacity: 1;
}

/* 이벤트 컨트롤러 hide (플러그인 옵션값 isUse:false 일 경우 .jd-slider에 slider--none 클래스 자동 추가됨.) */
.jd-slider.slider--none .prev, .slider.slider--none .next, .slider.slider--none .controller {
	display: none;
}
```
## JS
* 반응형 기본구조

```javascript
$('.slider1').jdSlider({
  slideShow: 2,
  slideToScroll: 2,
  responsive: [{
    viewSize: 768,
    setting: {
      slideShow: 1,
      slideToScroll: 1
    }
  }]
});
```

* 옵션값

```javascript
{
    isUse: true, // 슬라이더 적용 유무
    wrap: null, // 슬라이더 너비 계산의 기준이 될 선택자 (null일 경우 default.slide 선택자를 향한다.)
    slide: '.slide-area', // 슬라이드를 감싸는 선택자
    prev: '.prev', // 이전 버튼 선택자
    next: '.next', // 다음 버튼 선택자
    indicate: '.indicate-area', // 인디케이트 버튼을 감싸는 선택자
    auto: '.auto', // 자동재생 버튼 선택자
    playClass: 'play', // 자동재생 버튼에서의 재생 class
    pauseClass: 'pause', // 자동재생 버튼에서의 정지 class
    noSliderClass: 'slider--none', // 슬라이드 기능이 없는 경우 나타낼 class
    willFocusClass: 'will-focus', // 버튼 이동 접근성을 위한 class
    unusedClass: 'hidden', // 사용되지 않는 prev, next 버튼에 추가될 class
    slideShow: 1, // 화면에 보여질 슬라이드 수
    slideToScroll: 1, // 슬라이딩 되어지는 슬라이드 수
    slideStart: 1, // 시작 슬라이드 번호
    margin: null, // 슬라이드 간격 (margin-right와 동일하게 구성 필요 | null일 경우 style margin-right값을 따른다.)
    speed: 500, // 슬라이딩 속도 (1000 = 1초)
    timingFunction: 'ease', // ie9 이하 제외 적용 (transition-timing-function)
    easing: 'swing', // ie9 이하 적용 (animate easing)
    interval: 4000, // 자동재생 속도 (1000 = 1초)
    touchDistance: 20, // 스와이프 허용 거리
    resistanceRatio: .5, // 스와이프 저항 비율
    isOverflow: false, // 사이드 영역 노출 여부
    isIndicate: true, // 인디케이트 사용 유무
    isAuto: false, // 자동재생 유무 (true 시 자동재생 실행)
    isLoop: false, // 무한루프 유무
    isSliding: true, // 슬라이딩 유무(페이드인 적용시 false)
    isCursor: true, // 마우스오버에 따른 자동재생 조절 유무
    isTouch: true, // 모바일 터치 스와이프 유무
    isDrag: true, // 웹 드래깅 스와이프 유무
    isResistance: true, // 스와이프 저항 유무
    isCustomAuto: false, // 자동재생 수동 컨트롤 유무 (자동은 false)
    autoState: 'auto', // 리사이징시 재생상태 변환
    indicateList: function (i) {
        return '<a href="#">' + i + '</a>'; // 인디케이트 커스텀
    },
    progress: function () {}, // 슬라이딩 발생 즉시 함수
    callback: function () {}, // 슬라이딩 완료 후 함수
    onPrev: function () {}, // prev 버튼 클릭 시 콜백함수
    onNext: function () {}, // next 버튼 클릭 시 콜백함수
    onIndicate: function () {}, // indicate 버튼 클릭 시 콜백함수
    onAuto: function () {}, // auto 버튼 클릭 시 콜백함수
    responsive: [
        {
            viewSize: 768, // break point(0~768)
            settings: { // 해당 영역 options 설정
                isUse: true
            }
        }, {
            viewSize: 1024, // break point(769~1024)
            settings: { // 해당 영역 options 설정
                isUse: true
            }
        }
    ] // 반응형 옵션 설정
}
```

* 이벤트 트리거

```javascript
$(selector)
    .trigger('init') // 구조 초기화 (기존 옵션값대로 재설정이 필요할 경우)
    .trigger('update') // 사이즈 갱신 (슬라이더 적용 후 사이즈를 강제로 수정한 경우)
    .trigger('resizeFn') // 반응형 분기가 변경 되었을 시에는 init, 같은 분기일 시에는 update 실행.
    .trigger('removeFn') // 슬라이더 전체 기능 제거

$(indicateButton)
    .trigger('moveTo') // indicateButton 으로 이동 (progress, callback 미발생)
```