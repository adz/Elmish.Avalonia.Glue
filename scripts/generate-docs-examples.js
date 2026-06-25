const { spawnSync } = require('child_process');
const path = require('path');

const rootDir = path.resolve(__dirname, '..');

const command = 'dotnet run --project tools/DocsExamples/DocsExamples.fsproj';
const result = spawnSync('bash', ['-lc', command], {
  cwd: rootDir,
  stdio: 'inherit',
  shell: false,
});

process.exit(result.status ?? 1);
