import dayjs from "dayjs";

export function nameof<T>(key: keyof T): string {
    return key as string;
}

export function toStandardFormat(date: string): string {
    return dayjs(date).format('DD/MM/YYYY')
}

export function toStandardExtendedFormat(date: string): string {
    return dayjs(date).format('DD/MM/YYYY HH:mm')
}