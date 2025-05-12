// UTC tarih string'ini kullanıcının yerel saat dilimine göre formatlar.
export const convertUtcToLocalFormattedString = (utcDateString) => {
  if (!utcDateString) return ''; // Eğer tarih yoksa boş string dön

  const date = new Date(utcDateString);
  if (isNaN(date.getTime())) {
    return "Geçersiz Tarih";
  }

  const options = {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
    hour12: false
  };

  try {
    return date.toLocaleString(undefined, options);
  } catch (error) {
    console.error("Tarih formatlama hatası:", error);
    
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    return `${day}.${month}.${year} ${hours}:${minutes}`;
  }
};
