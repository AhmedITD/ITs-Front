<script>
  import QRCode from 'qrcode';

  export let payload = '';
  export let size = 200;

  let dataUrl = '';

  $: if (payload) {
    QRCode.toDataURL(payload, { width: size, margin: 2 })
      .then((url) => (dataUrl = url))
      .catch(() => (dataUrl = ''));
  }
</script>

{#if dataUrl}
  <div class="qr-wrap">
    <img src={dataUrl} alt="QR Code" width={size} height={size} class="qr-img" />
  </div>
{:else if payload}
  <p class="qr-loading">Generating QR…</p>
{/if}

<style>
  .qr-wrap {
    display: inline-flex;
    padding: 12px;
    background: white;
    border-radius: 12px;
  }
  .qr-img {
    display: block;
  }
  .qr-loading {
    color: #888;
    font-size: 0.9rem;
  }
</style>
