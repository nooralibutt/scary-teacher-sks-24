/*
 * Copyright 2016 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package org.gradle.api.plugins.quality;

import groovy.lang.Closure;
import groovy.lang.DelegatesTo;
import org.gradle.api.Action;
import org.gradle.api.file.DirectoryProperty;
import org.gradle.api.file.FileCollection;
import org.gradle.api.file.FileTree;
import org.gradle.api.internal.project.IsolatedAntBuilder;
import org.gradle.api.model.ObjectFactory;
import org.gradle.api.plugins.quality.internal.CheckstyleInvoker;
import org.gradle.api.plugins.quality.internal.CheckstyleReportsImpl;
import org.gradle.api.provider.Provider;
import org.gradle.api.reporting.Reporting;
import org.gradle.api.resources.TextResource;
import org.gradle.api.tasks.CacheableTask;
import org.gradle.api.tasks.Classpath;
import org.gradle.api.tasks.Console;
import org.gradle.api.tasks.Input;
import org.gradle.api.tasks.InputDirectory;
import org.gradle.api.tasks.Internal;
import org.gradle.api.tasks.Nested;
import org.gradle.api.tasks.Optional;
import org.gradle.api.tasks.PathSensitive;
import org.gradle.api.tasks.PathSensitivity;
import org.gradle.api.tasks.SourceTask;
import org.gradle.api.tasks.TaskAction;
import org.gradle.api.tasks.VerificationTask;
import org.gradle.internal.deprecation.DeprecationLogger;
import org.gradle.util.ClosureBackedAction;

import javax.annotation.Nullable;
import javax.inject.Inject;
import java.io.File;
import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Runs Checkstyle against some source files.
 */
@CacheableTask
public class Checkstyle extends SourceTask implements VerificationTask, Reporting<CheckstyleReports> {

    private FileCollection checkstyleClasspath;
    private FileCollection classpath;
    private TextResource config;
    private Map<String, Object> configProperties = new LinkedHashMap<String, Object>();
    private final CheckstyleReports reports;
    private boolean ignoreFailures;
    private int maxErrors;
    private int maxWarnings = Integer.MAX_VALUE;
    private boolean showViolations = true;
    private final DirectoryProperty configDirectory;

    /**
     * The Checkstyle configuration file to use.
     */
    @Internal
    public File getConfigFile() {
        return getConfig() == null ? null : getConfig().asFile();
    }

    /**
     * The Checkstyle configuration file to use.
     */
    public void setConfigFile(File configFile) {
        setConfig(getProject().getResources().getText().fromFile(configFile));
    }

    public Checkstyle() {
        configDirectory = getObjectFactory().directoryProperty();
        reports = getObjectFactory().newInstance(CheckstyleReportsImpl.class, this);
    }

    @Inject
    protected ObjectFactory getObjectFactory() {
        throw new UnsupportedOperationException();
    }

    @Inject
    public IsolatedAntBuilder getAntBuilder() {
        throw new UnsupportedOperationException();
    }

    /**
     * Configures the reports to be generated by this task.
     *
     * The contained reports can be configured by name and closures. Example:
     *
     * <pre>
     * checkstyleTask {
     *   reports {
     *     html {
     *       destination "build/checkstyle.html"
     *     }
     *   }
     * }
     * </pre>
     *
     * @param closure The configuration
     * @return The reports container
     */
    @Override
    public CheckstyleReports reports(@DelegatesTo(value = CheckstyleReports.class, strategy = Closure.DELEGATE_FIRST) Closure closure) {
        return reports(new ClosureBackedAction<CheckstyleReports>(closure));
    }

    /**
     * Configures the reports to be generated by this task.
     *
     * The contained reports can be configured by name and closures. Example:
     *
     * <pre>
     * checkstyleTask {
     *   reports {
     *     html {
     *       destination "build/checkstyle.html"
     *     }
     *   }
     * }
     * </pre>
     *
     * @param configureAction The configuration
     * @return The reports container
     * @since 3.0
     */
    @Override
    public CheckstyleReports reports(Action<? super CheckstyleReports> configureAction) {
        configureAction.execute(reports);
        return reports;
    }

    @TaskAction
    public void run() {
        CheckstyleInvoker.invoke(this);
    }

    /**
     * {@inheritDoc}
     *
     * <p>The sources for this task are relatively relocatable even though it produces output that
     * includes absolute paths. This is a compromise made to ensure that results can be reused
     * between different builds. The downside is that up-to-date results, or results loaded
     * from cache can show different absolute paths than would be produced if the task was
     * executed.</p>
     */
    @Override
    @PathSensitive(PathSensitivity.RELATIVE)
    public FileTree getSource() {
        return super.getSource();
    }

    /**
     * The class path containing the Checkstyle library to be used.
     */
    @Classpath
    public FileCollection getCheckstyleClasspath() {
        return checkstyleClasspath;
    }

    /**
     * The class path containing the Checkstyle library to be used.
     */
    public void setCheckstyleClasspath(FileCollection checkstyleClasspath) {
        this.checkstyleClasspath = checkstyleClasspath;
    }

    /**
     * The class path containing the compiled classes for the source files to be analyzed.
     */
    @Classpath
    public FileCollection getClasspath() {
        return classpath;
    }

    /**
     * The class path containing the compiled classes for the source files to be analyzed.
     */
    public void setClasspath(FileCollection classpath) {
        this.classpath = classpath;
    }

    /**
     * The Checkstyle configuration to use. Replaces the {@code configFile} property.
     *
     * @since 2.2
     */
    @Nested
    public TextResource getConfig() {
        return config;
    }

    /**
     * The Checkstyle configuration to use. Replaces the {@code configFile} property.
     *
     * @since 2.2
     */
    public void setConfig(TextResource config) {
        this.config = config;
    }

    /**
     * The properties available for use in the configuration file. These are substituted into the configuration file.
     */
    @Nullable
    @Optional
    @Input
    public Map<String, Object> getConfigProperties() {
        return configProperties;
    }

    /**
     * The properties available for use in the configuration file. These are substituted into the configuration file.
     */
    public void setConfigProperties(@Nullable Map<String, Object> configProperties) {
        this.configProperties = configProperties;
    }

    /**
     * Path to other Checkstyle configuration files.
     * <p>
     * This path will be exposed as the variable {@code config_loc} in Checkstyle's configuration files.
     * </p>
     *
     * @return path to other Checkstyle configuration files
     * @since 4.0
     */
    @Optional
    @PathSensitive(PathSensitivity.RELATIVE)
    @InputDirectory
    @Nullable
    @Deprecated
    // @ReplacedBy("configDirectory")
    public File getConfigDir() {
        // TODO: The annotations need to be moved to the new property
        DeprecationLogger.deprecateMethod(Checkstyle.class, "getConfigDir()").replaceWith("Checkstyle.getConfigDirectory()")
            .willBeRemovedInGradle7()
            .withDslReference(Checkstyle.class, "configDir")
            .nagUser();
        File configDir = getConfigDirectory().getAsFile().getOrNull();
        if (configDir != null && configDir.exists()) {
            return configDir;
        }
        return null;
    }

    /**
     * Path to other Checkstyle configuration files.
     * <p>
     * This path will be exposed as the variable {@code config_loc} in Checkstyle's configuration files.
     * </p>
     *
     * @since 4.0
     */
    @Deprecated
    public void setConfigDir(Provider<File> configDir) {
        DeprecationLogger.deprecateMethod(Checkstyle.class, "setConfigDir()").replaceWith("Checkstyle.getConfigDirectory().set()")
            .willBeRemovedInGradle7()
            .withDslReference(Checkstyle.class, "configDir")
            .nagUser();
        this.configDirectory.set(getProject().getLayout().dir(configDir));
    }

    /**
     * Path to other Checkstyle configuration files.
     * <p>
     * This path will be exposed as the variable {@code config_loc} in Checkstyle's configuration files.
     * </p>
     *
     * @return path to other Checkstyle configuration files
     * @since 6.0
     */
    @Internal
    public DirectoryProperty getConfigDirectory() {
        return configDirectory;
    }

    /**
     * The reports to be generated by this task.
     */
    @Override
    @Nested
    public final CheckstyleReports getReports() {
        return reports;
    }

    /**
     * Whether or not this task will ignore failures and continue running the build.
     *
     * @return true if failures should be ignored
     */
    @Override
    public boolean getIgnoreFailures() {
        return ignoreFailures;
    }

    /**
     * Whether this task will ignore failures and continue running the build.
     *
     * @return true if failures should be ignored
     */
    @Internal
    public boolean isIgnoreFailures() {
        return ignoreFailures;
    }

    /**
     * Whether this task will ignore failures and continue running the build.
     */
    @Override
    public void setIgnoreFailures(boolean ignoreFailures) {
        this.ignoreFailures = ignoreFailures;
    }

    /**
     * The maximum number of errors that are tolerated before breaking the build
     * or setting the failure property.
     *
     * @return the maximum number of errors allowed
     * @since 3.4
     */
    @Input
    public int getMaxErrors() {
        return maxErrors;
    }

    /**
     * Set the maximum number of errors that are tolerated before breaking the build.
     *
     * @param maxErrors number of errors allowed
     * @since 3.4
     */
    public void setMaxErrors(int maxErrors) {
        this.maxErrors = maxErrors;
    }

    /**
     * The maximum number of warnings that are tolerated before breaking the build
     * or setting the failure property.
     *
     * @return the maximum number of warnings allowed
     * @since 3.4
     */
    @Input
    public int getMaxWarnings() {
        return maxWarnings;
    }

    /**
     * Set the maximum number of warnings that are tolerated before breaking the build.
     *
     * @param maxWarnings number of warnings allowed
     * @since 3.4
     */
    public void setMaxWarnings(int maxWarnings) {
        this.maxWarnings = maxWarnings;
    }

    /**
     * Whether rule violations are to be displayed on the console.
     *
     * @return true if violations should be displayed on console
     */
    @Console
    public boolean isShowViolations() {
        return showViolations;
    }

    /**
     * Whether rule violations are to be displayed on the console.
     */
    public void setShowViolations(boolean showViolations) {
        this.showViolations = showViolations;
    }
}
