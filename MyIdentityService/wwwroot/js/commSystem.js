
Vue.component('com-menu', {
    props: ['UserName', 'CommentText', 'Comments', 'depth'],
    name: 'com-menu',
    computed: {
        indent() {
            return { transform: `translate(${this.depth * 50}px)` }
        }
    },
    template: `
<div class="com-menu">
<div :style="indent">
<li class="media mt-2">
                <div class="media-left">

                        <img class="media-object img-circle" src="/uploads/cat.jpg" alt="...">
                    
                </div>
                <div class="media-body">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <div class="author"><b>{{ UserName }}</b></div>
                            <div class="metadata">
                                <span class="date">${new Date()}</span>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div class="media-text text-justify">{{ CommentText }}</div>
                            <div class="pull-right"><a class="btn btn-info" id="replayBtn" onclick="jsReplay()">Replay</a></div>
                                <div id="replayText">
                                </div>
                        </div>
                    </div>
</li>
</div>
 <com-menu 
      v-for="com in Comments" 
      :Comments="com.Comments" 
      :UserName="com.UserName"
      :CommentText="com.CommentText"
      :depth="depth + 1"
    >
    </com-menu>
</div>


`
});

new Vue({
    el: "#appComment",
    data: function () {

        var p1 = document.getElementById('PassingToJavaScript1').value;
        // parse the serialized data
        return JSON.parse(p1);
    }

});

//new Vue({
//    el: '#vue-app',
//    data: function () {
//        // parse the serialized data
//        var modelJSValue = myData;
//        console.log(modelJSValue);
//        return modelJSValue;//JSON.parse('@data');
//    }
//});

//Vue.component('header-component', {
//    props: ['menuItems'], // pass in the menu from the app
//    template: '<header><ul><li v-for="item in menuItems">{{ item  }}</li</ul></header>'
//});
