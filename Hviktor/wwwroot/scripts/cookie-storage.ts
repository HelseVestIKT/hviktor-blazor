export function setCookie(key: string, value: string): void {
  const d = new Date();
  d.setTime(d.getTime() + 30 * 24 * 60 * 60 * 1000);
  const expires = "expires=" + d.toUTCString();
  document.cookie = `${key}=${value};${expires};path=/;SameSite=Lax`;
}

export function getCookie(key: string): string | null {
  const decodedCookie = decodeURIComponent(document.cookie);

  if (key == null || key === "") {
    return decodedCookie;
  }

  const name = key + "=";
  const cookieArr = decodedCookie.split(";");
  for (const cookie of cookieArr) {
    let item = cookie;
    while (item.startsWith(" ")) {
      item = item.substring(1);
    }
    if (item.startsWith(name)) {
      return item.substring(name.length, cookie.length);
    }
  }
  return null;
}

globalThis.Hviktor = globalThis.Hviktor || {};
globalThis.Hviktor.Storage = globalThis.Hviktor.Storage || {};
globalThis.Hviktor.Storage.setCookie = setCookie;
globalThis.Hviktor.Storage.getCookie = getCookie;
