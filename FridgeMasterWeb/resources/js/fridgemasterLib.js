function LoadItems()
{
    jQuery.ajax({
        type: 'GET',
        url: 'http://localhost:58684/WebHost/api/Product/list',
        dataType: 'json',
        cache: false,
        crossDomain: true,
        processData: true,
        success: function (data) {

            var products = data;

            //$('#productsList').listview().empty();
            $('#productsList').children('li').detach();

            $.each(products, function (i, item) {
                var einheit = GetEinheitStringFromId(products[i].Einheit)
                $('#productsList').append('<li><a id="' + products[i].Id + '" href="#pageDetails" data-transition="slideup">' + products[i].Name + '<div id="' + products[i].Id + '" class="ui-li-count">' + products[i].Menge + ' ' + einheit + '</div></a></li>');
            });
            2
            // Sort items
            var listview = $('#productsList'),
                listitems = listview.children('li');

            listitems.detach().sort(function (a, b) {
                var adata = $(a).find('a').text().toUpperCase();
                var bdata = $(b).find('a').text().toUpperCase();
                return (adata > bdata) ? 1 : -1;
            });

            listview.append(listitems);

            // Apply auto dividers
            $("#productsList").listview({
                autodividers: true,
                autodividersSelector: function (li) {
                    var out = li.find('a').text().charAt(0).toUpperCase();
                    return out;
                }
            }).listview('refresh');

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $('#dialogText').html('Function:LoadItems()<br/>' + 'Status:' + textStatus + '<br/>' + errorThrown);
            $.mobile.changePage("#connectionErrorDialog");
        }
    });
}

function SaveItem()
{
    var currentItem_Id = -1;
    var currentItem_Name = "no name";

    var currentItem_Einheit = "";
    var currentItem_Menge = 0;

    //GetId
    if (typeof $('#currentItemId').val() != 'undefined')
        currentItem_Id = $('#currentItemId').val();

    //GetEinheit
    $('#comboBox_Einheit option:selected').each(function () {
        currentItem_Einheit += $(this).val();
    });

    //GetMenge
    currentItem_Menge = $('#slider_Menge').val();

    //GetName
    currentItem_Name = $('#textBox_Name').val();


    var product = {
        Id: currentItem_Id,
        Name: currentItem_Name,
        Einheit: currentItem_Einheit,
        Menge: currentItem_Menge
    };

    jQuery.ajax({
        type: 'POST',
        url: 'http://localhost:58684/WebHost/api/Product/list',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        cache: false,
        crossDomain: true,
        processData: true,
        data: JSON.stringify(product),
        success: function () {

            //LoadItems();
            $.mobile.changePage("#pageMain");
            location.reload();

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $('#dialogText').html('Function:SaveItem()<br/>' + 'Status:' + textStatus + '<br/>' + errorThrown);
            $.mobile.changePage("#connectionErrorDialog");
        }

    });
}

function DeleteItem()
{
    if (typeof $('#currentItemId').val() != 'undefined')
    {
        var currentItem_Id = $('#currentItemId').val();

        jQuery.ajax({
            type: 'DELETE',
            url: 'http://localhost:58684/WebHost/api/Product/single/' + currentItem_Id,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: false,
            crossDomain: true,
            processData: true,
            success: function () {

                $('#currentItemId').val('');
                LoadItems();
                $.mobile.changePage("#pageMain");
                location.reload();

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#dialogText').html('Function:DeleteItem()<br/>' + 'Status:' + textStatus + '<br/>' + errorThrown);
                $.mobile.changePage("#connectionErrorDialog");
            }

        });

    }
    else
    {
        $('#dialogText').html('Dieses Produkt wurde bereits entfernt.');
        $.mobile.changePage("#connectionErrorDialog");
    }

    $('#currentItemId').val('');
    LoadItems();
    $.mobile.changePage("#pageMain");
    location.reload();
}

function SetupDetailsPage(id)
{
    if (typeof id == 'undefined')
    {
        id = $('#currentItemId').val();
    }

    jQuery.ajax({
        type: 'GET',
        url: 'http://localhost:58684/WebHost/api/Product/single/' + id,
        dataType: 'json',
        cache: false,
        crossDomain: true,
        processData: true,
        success: function (data) {

            var product = data;

            //Set Id
            $('#currentItemId').attr('value', data.Id);

            //Set Name
            $('#textBox_Name').attr('value', data.Name);

            //Set Einheit
            $('#comboBox_Einheit').val(data.Einheit);
            $('#comboBox_Einheit').selectmenu('refresh');

            //Set Menge
            SetupSlider(data.Einheit);
            $('#slider_Menge').val(data.Menge);
            $('#slider_Menge').slider('refresh');

            $('#delete_button').show();

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $('#dialogText').html('Function:GetProduct()<br/>' + 'Status:' + textStatus + '<br/>' + errorThrown);
            $.mobile.changePage("#connectionErrorDialog");
        }
    });
    
}

function ClearDetailsPage()
{
        $('#delete_button').hide();

        //Set Id
        $('#currentItemId').attr('value', -1);

        //Set Name
        $('#textBox_Name').val('');

        //Set Menge
        SetupSlider(0);
        $('#slider_Menge').val(0);
        
        //Set Einheit
        $('#comboBox_Einheit').val(0);

    try
    {
        $('#textBox_Name').textinput("refresh");
        $('#slider_Menge').slider('refresh');
        $('#comboBox_Einheit').selectmenu('refresh');
    }
    catch (errorThrown)
    {
        //$('#dialogText').html(errorThrown);
        //$.mobile.changePage("#connectionErrorDialog");
    }
}

function SetupSlider(id)
{
    var einheit = '';

    if (typeof id == 'undefined')
    {
        $('#comboBox_Einheit option:selected').each(function () {
            einheit += $(this).val();
        });
    }
    else
        einheit = id.toString();

	switch(einheit)
	{
		case '0':
		    $('#slider_Menge').attr('max', '5000');
		break;
		
	    case '1':
	        $('#slider_Menge').attr('max', '100');
		break;
		
		case '2':
		    $('#slider_Menge').attr('max', '50');
		break;
		
		case '3':
		    $('#slider_Menge').attr('max', '5');
		break;
    }

	if (typeof id == 'undefined')
	{
	    $('#slider_Menge').slider('refresh');
	}
}

function  GetEinheitStringFromId(einheit)
{
	switch(einheit)
	{
		case 0:
			return 'g';
		break;
			
		case 1:
			return 'St.';
		break;
		
		case 2:
			return 'Pck.';
		break;
		
		case 3:
			return 'kg';
		break;
	}
}