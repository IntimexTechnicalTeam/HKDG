$.widget( "dest.combobox", {
      _create: function() {
        this.wrapper = $( "<span>" )
          .addClass( "dest-combobox" )
          .insertAfter( this.element );
 
        this.element.hide();
        this._createAutocomplete();
        this._createShowAllButton();
      },
 
      _createAutocomplete: function() {
        var selected = this.element.children( ":selected" ),
          value = selected.val() ? selected.text() : "";
 
        this.input = $( "<input>" )
          .appendTo( this.wrapper )
          .val( value )
          .attr({"title": "", "name": this.element.attr('name'), "type": "text"})
          .addClass( "dest-combobox-input input-text ui-widget ui-widget-content ui-state-default ui-corner-left" )
          .autocomplete({
            delay: 0,
            minLength: 0,
            source: $.proxy( this, "_source" )
          })
          .on( "click", function() {$( this ).val(''); })
          .on( "mousedown", function() {
              $(this).autocomplete( "search", "" );
          })
          .tooltip({
            tooltipClass: "ui-state-highlight"
          });
 
        this._on( this.input, {
          autocompleteselect: function( event, ui ) {
            ui.item.option.selected = true;
            this._trigger( "select", event, {
              item: ui.item.option
            });
          },
          autocompleteopen: function(event, ui) {
            this._trigger( "open", event, ui);
          },
 
          autocompletechange: "_removeIfInvalid"
        });
      },
 
      _createShowAllButton: function() {
        var input = this.input,
          wasOpen = false;
 
        $( "<a>" )
          .attr( "tabIndex", -1 )
          // .attr( "title", "Show All Items" )
          .tooltip()
          .appendTo( this.wrapper )
          .button({
            icons: {
              primary: "ui-icon-triangle-1-s"
            },
            text: false
          })
          .removeClass( "ui-corner-all" )
          .addClass( "dest-combobox-toggle ui-corner-right" )
          .mousedown(function() {
            wasOpen = input.autocomplete( "widget" ).is( ":visible" );
          })
          .click(function() {
            input.focus();
 
            // ???????????????????????????
            if ( wasOpen ) {
              return;
            }
 
            // ????????????????????????????????????????????????????????????
            input.autocomplete( "search", "" );
          });
      },
 
      _source: function( request, response ) {
        var inputStr = request.term.toLowerCase();
        var searchCN = ('china'.indexOf(inputStr) >=0 || '??????'.indexOf(inputStr) >=0 || '??????'.indexOf(inputStr) >=0);

        var matcher = new RegExp( $.ui.autocomplete.escapeRegex(request.term), "i" );
        response( this.element.children( "option" ).map(function() {
          var text = $( this ).text();
          if ( (searchCN && text.toLowerCase().indexOf('mainland')>=0) || this.value && ( !request.term || matcher.test(text) ) )
            return {
              label: text,
              value: text,
              option: this
            };
        }) );
      },
 
      _removeIfInvalid: function( event, ui ) {
 
        // ????????????????????????????????????
        if ( ui.item ) {
          return;
        }
 
        // ??????????????????????????????????????????
        var value = this.input.val(),
          valueLowerCase = value.toLowerCase(),
          valid = false;
        this.element.children( "option" ).each(function() {
          if ( $( this ).text().toLowerCase() === valueLowerCase ) {
            this.selected = valid = true;
            return false;
          }
        });
 
        // ??????????????????????????????????????????
        if ( valid ) {
          return;
        }
 
        // ??????????????????
        this.input
          .val( "" );
          // .attr( "title", value + " didn't match any item" )
          // .tooltip( "open" );
        this.element.val( "" );
        // this._delay(function() {
        //   this.input.tooltip( "close" ).attr( "title", "" );
        // }, 2500 );
        this.input.data( "ui-autocomplete" ).term = "";
      },
 
      _destroy: function() {
        this.wrapper.remove();
        this.element.show();
      }
    });