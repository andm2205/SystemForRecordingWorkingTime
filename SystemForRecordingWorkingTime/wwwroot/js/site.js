AjaxRequest = (viewItemId, url, type = 'GET', data = null) => {
    if (typeof (viewItemId) === 'undefined' || typeof (url) === 'undefined')
        throw new Error('Required argument omitted')
    if (typeof(data) === 'HTMLFormElement')
        data = new FormData(data)
    try {
        $.ajax({
            type: type,
            data: data,
            url: url,
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.error === true) {
                    console.log(res.message)
                    alert(res.message)
                }
                else
                    $(viewItemId).html(res.html)
                $(viewItemId).h
            },
            error: function (err) {
                console.log(err)
                alert(err.message)
            }
        })
        return false;
    } catch (ex) {
        console.log(ex)
        alert(err.message)
    }
}