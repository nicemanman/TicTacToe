window.ShowWinnerAlert = (player) => {
    Swal.fire({
        title: player + ' won',
        width: 350,
        padding: '3em',
        color: '#716add',
        backdrop: `
                        rgba(0,0,123,0.4)
                        url("/images/nyan-cat-nyan.gif")
                        left top
                        no-repeat
                      `
    })
}
window.ShowTieAlert = () => {
    Swal.fire({
        title: 'It is a tie',
        width: 350,
        padding: '3em',
        color: '#716add',
        backdrop: `
                        rgba(0,0,123,0.4)
                        url("/images/crying-tear.gif")
                        left top
                        no-repeat
                      `
    })
}