
//copy to console in firefox and run it ... make sure [serviceURL] points to correct local url .


        Podium={};Podium.keydown=function(e){var t=document.createEvent("KeyboardEvent");Object.defineProperty(t,"keyCode",{get:function(){return this.keyCodeVal}});Object.defineProperty(t,"which",{get:function(){return this.keyCodeVal}});if(t.initKeyboardEvent){t.initKeyboardEvent("keydown",true,true,document.defaultView,false,false,false,false,e,e)}else{t.initKeyEvent("keydown",true,true,document.defaultView,false,false,false,false,e,0)}t.keyCodeVal=e;if(t.keyCode!==e){alert("keyCode mismatch "+t.keyCode+"("+t.which+")")}document.dispatchEvent(t)}
        var transition_timer; var serviceURL = 'http://localhost:41164/home/getbestmove';
        function transition1(){
            clearTimeout(transition_timer);

            var arrTile = new Array();
            $.each( $('div.tile-container div.tile'), function( key, value ) {
                var elem = this;
                var classList =$(this).attr('class').split(/\s+/);
                $.each( classList, function(index, item){
                    if (item.indexOf('tile-position-') == 0) {
                        var tilepos = item.replace('tile-position-', '');
                        var d = (tilepos+'-'+$(elem).find('div').html());
                        //alert(d);
                        arrTile[arrTile.length] = d;
                    }
                });
            });

            $.ajax({
                dataType: "json", traditional: true,
                url: serviceURL,
                data: {arr : arrTile},
                success: function(data){
                    //0 - Left, 1-  Right, 2 - Up, 3 - Down
                    switch(data) {
                        case 0:Podium.keydown(37);break;
                        case 1:Podium.keydown(39);break;
                        case 2:Podium.keydown(38);break;
                        case 3:Podium.keydown(40);break;
                        default:console.log(data);
                    }

                    if ($("div.game-message").is(":visible") == true)
                    {
                        if ($("a.keep-playing-button").is(":visible") == true)
                        {
                            jQuery('a.keep-playing-button')[0].click();
                        }
                        else if ($("a.continue-button").is(":visible") == true)
                        {
                            jQuery('a.continue-button')[0].click();
                        }
                        else if ($("a.retry-button").is(":visible") == true)//Try again game over
                        {
                            jQuery('a.retry-button')[0].click();
                        }
                    }
                    transition_timer = setTimeout(transition1, 700);
                    
                }
            });

            
        }
        transition1();

        
