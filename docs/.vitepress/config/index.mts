import { defineConfig } from 'vitepress'
import { shared } from './shared'
import { en_US } from './en_US'
import { ja_JP } from './ja_JP'
import { zh_Hans } from './zh_Hans'

export default defineConfig({
  ...shared,
  locales: {
    en_US: { label: 'English', ...en_US },
    ja_JP: { label: '日本語', ...ja_JP },
    zh_Hans: { label: '中文', ...zh_Hans },
  }
})
