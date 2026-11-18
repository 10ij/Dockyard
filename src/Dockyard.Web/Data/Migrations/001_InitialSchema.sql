PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS users (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    oidc_sub TEXT NOT NULL UNIQUE,
    username TEXT NOT NULL,
    email TEXT NOT NULL,
    is_admin INTEGER NOT NULL DEFAULT 0 CHECK (is_admin IN (0, 1)),
    created_at TEXT NOT NULL DEFAULT (strftime('%Y-%m-%dT%H:%M:%fZ', 'now')),
    last_login_at TEXT NULL
);

CREATE INDEX IF NOT EXISTS idx_users_oidc_sub ON users(oidc_sub);

CREATE TABLE IF NOT EXISTS apps (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    container_id TEXT NOT NULL,
    container_name TEXT NOT NULL UNIQUE,
    image TEXT NOT NULL,
    version TEXT NOT NULL DEFAULT '',
    status TEXT NOT NULL DEFAULT 'unknown',
    state TEXT NOT NULL DEFAULT 'created',
    ports_json TEXT NOT NULL DEFAULT '[]',
    metadata_json TEXT NOT NULL DEFAULT '{}',
    updated_at TEXT NOT NULL DEFAULT (strftime('%Y-%m-%dT%H:%M:%fZ', 'now'))
);

CREATE INDEX IF NOT EXISTS idx_apps_container_name ON apps(container_name);

CREATE TABLE IF NOT EXISTS container_permissions (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER NOT NULL,
    container_name TEXT NOT NULL,
    can_view INTEGER NOT NULL DEFAULT 0 CHECK (can_view IN (0, 1)),
    can_start INTEGER NOT NULL DEFAULT 0 CHECK (can_start IN (0, 1)),
    can_stop INTEGER NOT NULL DEFAULT 0 CHECK (can_stop IN (0, 1)),
    UNIQUE (user_id, container_name),
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    FOREIGN KEY (container_name) REFERENCES apps(container_name) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_permissions_user_container
    ON container_permissions(user_id, container_name);

CREATE TABLE IF NOT EXISTS audit_logs (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    created_at TEXT NOT NULL DEFAULT (strftime('%Y-%m-%dT%H:%M:%fZ', 'now')),
    action TEXT NOT NULL,
    message TEXT NOT NULL,
    severity TEXT NOT NULL DEFAULT 'info',
    username TEXT NOT NULL DEFAULT '',
    oidc_sub TEXT NOT NULL DEFAULT '',
    container_name TEXT NULL,
    ip_address TEXT NOT NULL DEFAULT '',
    user_agent TEXT NOT NULL DEFAULT '',
    metadata_json TEXT NOT NULL DEFAULT '{}'
);

CREATE INDEX IF NOT EXISTS idx_audit_logs_created_at ON audit_logs(created_at DESC);
CREATE INDEX IF NOT EXISTS idx_audit_logs_container ON audit_logs(container_name);